using DTOs.Payments;
using Interfaces.Audit;
using Interfaces.Email;
using Interfaces.Payments;
using Stripe.Checkout;
using System.Text.Json;

namespace Services.Payments;

public partial class StripeService(
    AppConfiguration configuration,
    IUnitOfWork unitOfWork,
    IOutboxWriter outboxWriter,
    ICurrentUserService currentUserService,
    IAuditLogWriter auditLogWriter,
    IStripeCheckoutGateway stripeCheckoutGateway) : IStripeService
{
    private readonly AppConfiguration _configuration = configuration;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOutboxWriter _outboxWriter = outboxWriter;
    private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IStripeCheckoutGateway _stripeCheckoutGateway = stripeCheckoutGateway;

    private readonly IGenericRepository<EducationAccount> _accountRepository = unitOfWork.Repository<EducationAccount>();
    private readonly IGenericRepository<Charge> _chargeRepository = unitOfWork.Repository<Charge>();
    private readonly IGenericRepository<Payment> _paymentRepository = unitOfWork.Repository<Payment>();
    private readonly IGenericRepository<PaymentAllocation> _paymentAllocationRepository = unitOfWork.Repository<PaymentAllocation>();
    private readonly IGenericRepository<EducationCreditTransaction> _transactionRepository = unitOfWork.Repository<EducationCreditTransaction>();
    private readonly IGenericRepository<ChargeInstallment> _installmentRepository = unitOfWork.Repository<ChargeInstallment>();
    private readonly IGenericRepository<ApplicationSetting> _applicationSettingRepository = unitOfWork.Repository<ApplicationSetting>();

    private record BillingItem(Charge Charge, PaymentIntent Intent, int? PaymentPlanMonths, int? TargetInstallmentId, decimal AmountToPay);
    private record ChargeBillingActionItem(
        int ChargeId,
        PaymentIntent Intent,
        int? PaymentPlanMonths,
        int? InstallmentCount);
    private record PaymentSplit(decimal TotalOwed, decimal BalanceToApply, decimal StripeAmount);
    private record ReservedPaymentResult(
        List<Payment>? Payments,
        PaymentSessionResponseDTO? ExistingSession,
        Session? PaidSession = null);
    private record PendingSessionLookup(
        PaymentSessionResponseDTO? ExistingSession,
        Session? PaidSession);
    private record PaymentProcessingTotals(decimal CreditBalanceCovered, decimal OnlinePaymentCovered);

    // ===== Cụm 1: Payment action endpoints =====
    // Public action endpoints only translate request DTOs into a single internal action list.
    // The endpoint itself decides the action type, so one request cannot mix payment intents.
    public Task<PaymentSessionResponseDTO> PayFullChargesAsync(
        PayFullChargesRequest request,
        CancellationToken cancellationToken = default)
    {
        var requestItems = request.ChargeIds
            .Select(chargeId => new ChargeBillingActionItem(chargeId, PaymentIntent.PayFull, null, null))
            .ToList();

        return HandlePaymentSessionAsync(requestItems, request.CreditBalanceApplied, cancellationToken);
    }

    public Task<PaymentSessionResponseDTO> CreateInstallmentPlansAsync(
        CreateInstallmentPlansRequest request,
        CancellationToken cancellationToken = default)
    {
        var requestItems = request.Items
            .Select(item => new ChargeBillingActionItem(item.ChargeId, PaymentIntent.CreateInstallment, item.PaymentPlanMonths, null))
            .ToList();

        return HandlePaymentSessionAsync(requestItems, request.CreditBalanceApplied, cancellationToken);
    }

    public Task<PaymentSessionResponseDTO> PayDueInstallmentsAsync(
        PayDueInstallmentsRequest request,
        CancellationToken cancellationToken = default)
    {
        var requestItems = request.Items
            .Select(item => new ChargeBillingActionItem(
                item.ChargeId,
                PaymentIntent.PayDueInstallments,
                null,
                item.InstallmentCount))
            .ToList();

        return HandlePaymentSessionAsync(requestItems, request.CreditBalanceApplied, cancellationToken);
    }

    public Task<PaymentSessionResponseDTO> PayRemainingInstallmentsAsync(
        PayRemainingInstallmentsRequest request,
        CancellationToken cancellationToken = default)
    {
        var requestItems = request.ChargeIds
            .Select(chargeId => new ChargeBillingActionItem(chargeId, PaymentIntent.PayRemainingInstallments, null, null))
            .ToList();

        return HandlePaymentSessionAsync(requestItems, request.CreditBalanceApplied, cancellationToken);
    }

    // ===== Cụm 2: Checkout orchestration =====
    // Main flow: load current account, load selected charges, validate action rules,
    // reserve Payment/Allocation rows, then either finalize balance-only or create Stripe checkout.
    private async Task<PaymentSessionResponseDTO> HandlePaymentSessionAsync(
        List<ChargeBillingActionItem> chargeBillingActionItems,
        decimal creditBalanceApplied,
        CancellationToken cancellationToken = default)
    {
        var chargeIds = chargeBillingActionItems.Select(c => c.ChargeId).ToList();
        var educationAccount = await GetCurrentEducationAccountAsync(cancellationToken);
        var charges = await GetPayableChargesAsync(educationAccount.Id, chargeIds, cancellationToken);

        // Validate action-specific business rules before any payment row is reserved.
        var utcNow = DateTime.UtcNow;
        ValidateBillingActions(chargeBillingActionItems, charges, educationAccount, creditBalanceApplied, utcNow);

        var billingItems = CreateBillingItems(chargeBillingActionItems, charges, utcNow);
        var split = CreatePaymentSplit(billingItems, creditBalanceApplied);

        var reservation = await ReservePaymentsForCheckoutAsync(
            chargeIds, chargeBillingActionItems, charges, billingItems,
            educationAccount, split, cancellationToken);

        if (reservation.PaidSession != null)
        {
            await ProcessStripeSessionAsync(
                educationAccount.Id,
                reservation.PaidSession,
                PaymentStatus.Succeeded,
                cancellationToken);
            return await BuildSessionResponseAsync(
                reservation.PaidSession.Id,
                PaymentStatus.Succeeded,
                cancellationToken);
        }

        if (reservation.ExistingSession != null) return reservation.ExistingSession;

        var payments = reservation.Payments!;

        var stripePayment = payments!.FirstOrDefault(p => p.PaymentMethod == PaymentMethod.OnlinePayment);
        var walletPayment = payments!.FirstOrDefault(p => p.PaymentMethod == PaymentMethod.EducationBalance);

        // Balance-only payment is finalized internally; no Stripe session or zero-amount line item is created.
        if (split.StripeAmount <= 0 && walletPayment != null)
        {
            return await CompleteBalanceOnlyPaymentAsync(walletPayment, educationAccount.Id, chargeBillingActionItems, cancellationToken);
        }

        if (stripePayment == null)
            throw new InternalAppException($"Failed to generate stripe {nameof(Payment)} record.");

        return await CreateStripeCheckoutSessionAsync(
            stripePayment, walletPayment, educationAccount, payments!, billingItems,
            chargeBillingActionItems, split, cancellationToken);
    }

    private async Task<EducationAccount> GetCurrentEducationAccountAsync(CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        var educationAccount = await _accountRepository.Query(tracking: false)
            .Include(a => a.Citizen)
            .FirstOrDefaultAsync(a => a.Citizen != null && a.Citizen.User != null && a.Citizen.User.Id == currentUserId, cancellationToken);

        return educationAccount ?? throw new InternalAppException("Current account holder not found!");
    }

    private async Task<List<Charge>> GetPayableChargesAsync(
        int educationAccountId, List<int> chargeIds, CancellationToken cancellationToken)
    {
        return await _chargeRepository.Query(tracking: false)
            .Include(c => c.Enrollment)
            .ThenInclude(e => e.Course)
            .Include(c => c.Installments.Where(isn => isn.Status != ChargeInstallmentStatus.Paid).OrderBy(isn => isn.DueDate))
            .Where(c => c.Enrollment.SchoolStudent.EducationAccountId == educationAccountId && chargeIds.Contains(c.Id))
            .OrderBy(c => c.Enrollment.Course.StartDate)
            .ToListAsync(cancellationToken);
    }

    private static PaymentSplit CreatePaymentSplit(List<BillingItem> billingItems, decimal creditBalanceApplied)
    {
        var totalOwed = billingItems.Sum(item => item.AmountToPay);
        var balanceToApply = Math.Min(creditBalanceApplied, totalOwed);
        return new PaymentSplit(totalOwed, balanceToApply, totalOwed - balanceToApply);
    }

    private async Task<ReservedPaymentResult> ReservePaymentsForCheckoutAsync(
        List<int> chargeIds,
        List<ChargeBillingActionItem> chargeBillingActionItems,
        List<Charge> charges,
        List<BillingItem> billingItems,
        EducationAccount educationAccount,
        PaymentSplit split,
        CancellationToken cancellationToken)
    {
        // Reserve Payment/Allocation rows in one transaction before Stripe is called.
        // This prevents two pending sessions from locking the same charge concurrently.
        return await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
        {
            await LockEducationAccountAsync(educationAccount.Id, token);

            var pendingSession = await FindMatchingActivePendingSessionAsync(
                chargeBillingActionItems, token);
            if (pendingSession.PaidSession != null)
                return new ReservedPaymentResult(null, null, pendingSession.PaidSession);
            if (pendingSession.ExistingSession != null)
                return new ReservedPaymentResult(null, pendingSession.ExistingSession);

            var newPayments = await CreatePrePaymentAndPaymentAllocationsAsync(
                charges, billingItems, educationAccount,
                split.TotalOwed, split.BalanceToApply, split.StripeAmount, token);

            SetReservationReferences(newPayments);

            return new ReservedPaymentResult(newPayments, null);
        }, cancellationToken);
    }

    private static void SetReservationReferences(List<Payment> payments)
    {
        var stripePayment = payments.FirstOrDefault(p => p.PaymentMethod == PaymentMethod.OnlinePayment);
        var walletPayment = payments.FirstOrDefault(p => p.PaymentMethod == PaymentMethod.EducationBalance);

        var reservationId = "RESERVING_" + Guid.NewGuid().ToString("N");
        stripePayment?.ExternalReference = reservationId;
        walletPayment?.ExternalReference = reservationId + "_wallet";
    }

    private async Task<PaymentSessionResponseDTO> CompleteBalanceOnlyPaymentAsync(
        Payment walletPayment,
        int educationAccountId,
        List<ChargeBillingActionItem> chargeBillingActionItems,
        CancellationToken cancellationToken)
    {
        await ProcessPaymentInternalAsync(
            [walletPayment.Id], educationAccountId, null, chargeBillingActionItems,
            targetStatus: PaymentStatus.Succeeded, cancellationToken);

        return PaymentSessionResponseFactory.FromPayments([walletPayment], PaymentStatus.Succeeded, link: "");
    }

    private async Task<PaymentSessionResponseDTO> CreateStripeCheckoutSessionAsync(
        Payment stripePayment,
        Payment? walletPayment,
        EducationAccount educationAccount,
        List<Payment> payments,
        List<BillingItem> billingItems,
        List<ChargeBillingActionItem> chargeBillingActionItems,
        PaymentSplit split,
        CancellationToken cancellationToken)
    {
        var options = CreateSessionOptions(
            educationAccount,
            billingItems,
            chargeBillingActionItems,
            split.BalanceToApply);

        var session = await ValidateStripeSession(stripePayment, walletPayment, options, cancellationToken);

        return await BindSessionAndReturnUrlAsync(stripePayment, walletPayment, session, cancellationToken);
    }

    private SessionCreateOptions CreateSessionOptions(
        EducationAccount educationAccount,
        List<BillingItem> billingItems,
        List<ChargeBillingActionItem> chargeBillingActionItems,
        decimal balanceToApply)
    {
        var lineItems = new List<SessionLineItemOptions>();
        CreateItemsCheckOut(billingItems, balanceToApply, lineItems);

        var sessionMetadataDto = new StripeSessionMetadataDTO
        {
            AccountId = educationAccount.Id,
            BillingActions = chargeBillingActionItems.Select(item => new StripeSessionBillingActionDTO
            {
                ChargeId = item.ChargeId,
                Intent = item.Intent,
                PaymentPlanMonths = item.PaymentPlanMonths,
                InstallmentCount = item.InstallmentCount
            }).ToList()
        };

        return new SessionCreateOptions
        {
            PaymentMethodTypes = [_configuration.StripeConfig.Method],
            LineItems = lineItems,
            Mode = _configuration.StripeConfig.Mode,
            SuccessUrl = _configuration.StripeConfig.SuccessUrl,
            CancelUrl = _configuration.StripeConfig.CancelUrl,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_configuration.StripeConfig.SessionExpiryMinutes),
            CustomerEmail = educationAccount!.Citizen.Email ?? "",
            Metadata = new Dictionary<string, string>
            {
                { "sessionData", JsonSerializer.Serialize(sessionMetadataDto) }
            }
        };
    }

    private async Task LockEducationAccountAsync(int educationAccountId, CancellationToken cancellationToken)
    {
        try
        {
            // Lock the EducationAccount row to serialize concurrent checkout requests for this user.
            await _accountRepository.Query(tracking: false)
                .Where(a => a.Id == educationAccountId)
                .ExecuteUpdateAsync(s => s.SetProperty(a => a.Status, a => a.Status), cancellationToken);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("ExecuteUpdate", StringComparison.Ordinal))
        {
            await _accountRepository.Query(tracking: true)
                .FirstAsync(a => a.Id == educationAccountId, cancellationToken);
        }
    }

    // Mark reservations as failed when Stripe cannot create a session so they no longer lock the charge.
    private async Task<Session> ValidateStripeSession(
        Payment stripePayment, Payment? walletPayment,
        SessionCreateOptions options, CancellationToken cancellationToken)
    {
        Session session;
        try
        {
            session = await _stripeCheckoutGateway.CreateAsync(options, cancellationToken);
        }
        catch
        {
            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                var paymentIds = walletPayment == null
                    ? new[] { stripePayment.Id }
                    : new[] { stripePayment.Id, walletPayment.Id };
                var reservedPayments = await _paymentRepository.Query(tracking: true)
                    .Where(payment => paymentIds.Contains(payment.Id))
                    .ToListAsync(token);

                foreach (var payment in reservedPayments)
                    payment.Status = PaymentStatus.Failed;

            }, cancellationToken);
            throw;
        }

        return session;
    }

    //Gắn external reference cho payments để kiểm tra trong trường hợp người dùng spam tạo link cho cùng 1 list charges và trả về link thanh toán
    private async Task<PaymentSessionResponseDTO> BindSessionAndReturnUrlAsync(Payment stripePayment, Payment? walletPayment, Session session, CancellationToken cancellationToken)
    {

        await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
        {
            var trackedStripePayment = await _paymentRepository
                .Query(tracking: true)
                .FirstAsync(p => p.Id == stripePayment.Id, token);

            trackedStripePayment.ExternalReference = session.Id;

            if (walletPayment != null)
            {
                var trackedWalletPayment = await _paymentRepository
                    .Query(tracking: true)
                    .FirstAsync(p => p.Id == walletPayment.Id, token);

                trackedWalletPayment.ExternalReference = session.Id + "_wallet";
            }

        }, cancellationToken);

        var responsePayments = walletPayment == null
            ? [stripePayment]
            : new List<Payment> { walletPayment, stripePayment };

        return PaymentSessionResponseFactory.FromPayments(responsePayments, PaymentStatus.Pending, session.Url);
    }

}

internal static class PaymentSessionResponseFactory
{
    public static PaymentSessionResponseDTO FromPayments(
        IEnumerable<Payment> payments,
        PaymentStatus status,
        string? link = null)
    {
        var paymentList = payments
            .Where(payment => payment != null)
            .OrderBy(payment => payment.PaymentMethod)
            .ToList();
        var walletAmount = paymentList
            .Where(payment => payment.PaymentMethod == PaymentMethod.EducationBalance)
            .Sum(payment => payment.TotalAmount);
        var onlineAmount = paymentList
            .Where(payment => payment.PaymentMethod == PaymentMethod.OnlinePayment)
            .Sum(payment => payment.TotalAmount);
        var paymentMode = (walletAmount, onlineAmount) switch
        {
            (> 0m, > 0m) => "Mixed",
            (> 0m, _) => "WalletOnly",
            (_, > 0m) => "OnlineOnly",
            _ => "None"
        };

        return new PaymentSessionResponseDTO
        {
            Link = link ?? string.Empty,
            Status = status.ToString(),
            PaymentMode = paymentMode,
            PaymentIds = paymentList.Select(payment => payment.Id).ToList(),
            TotalAmount = walletAmount + onlineAmount,
            WalletAmount = walletAmount,
            OnlineAmount = onlineAmount,
            IsWalletOnly = paymentMode == "WalletOnly",
            RequiresRedirect = !string.IsNullOrWhiteSpace(link)
        };
    }
}

