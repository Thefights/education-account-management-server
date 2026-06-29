using BLL.Interfaces.Payments;
using DTOs.Payments;
using Interfaces.Audit;
using Interfaces.Email;
using Stripe;
using Stripe.Checkout;
using System.Text.Json;
using PaymentMethod = Enums.PaymentMethod;

namespace BLL.Services.Payments;

public class StripeService(
    AppConfiguration configuration,
    IUnitOfWork unitOfWork,
    IOutboxWriter outboxWriter,
    IEmailService emailService,
    ICurrentUserService currentUserService,
    IAuditLogWriter auditLogWriter) : IStripeService
{
    private readonly AppConfiguration _configuration = configuration;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOutboxWriter _outboxWriter = outboxWriter;
    private readonly IEmailService _emailService = emailService;
    private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    private readonly IGenericRepository<EducationAccount> _accountRepository = unitOfWork.Repository<EducationAccount>();
    private readonly IGenericRepository<Models.Charge> _chargeRepository = unitOfWork.Repository<Models.Charge>();
    private readonly IGenericRepository<Payment> _paymentRepository = unitOfWork.Repository<Payment>();
    private readonly IGenericRepository<PaymentAllocation> _paymentAllocationRepository = unitOfWork.Repository<PaymentAllocation>();
    private readonly IGenericRepository<EducationCreditTransaction> _transactionRepository = unitOfWork.Repository<EducationCreditTransaction>();
    private readonly IGenericRepository<ChargeInstallment> _installmentRepository = unitOfWork.Repository<ChargeInstallment>();

    private record BillingItem(Models.Charge Charge, int? TargetInstallmentId, decimal AmountToPay);

    /// <summary>
    /// Khởi tạo phiên thanh toán (Checkout Session) lên Stripe.
    /// Hàm này tính toán số tiền cần thu, cấn trừ ví (Wallet), và sinh ra dữ liệu Payment chờ (Pending).
    /// </summary>
    public async Task<PaymentSessionResponseDTO> CreateCheckoutSessionAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        var sessionService = new SessionService();
        var accountId = _currentUserService.UserId;

        var chargePaymentRequestInfors = request.ChargePaymentRequestInfors;
        var chargeIds = chargePaymentRequestInfors.Select(c => c.ChargeId).ToList();

        var charges = await _chargeRepository.Query(tracking: false)
            .Include(c => c.Enrollment)
            .ThenInclude(e => e.Course)
            .Include(c => c.Installments.Where(isn => isn.Status == ChargeInstallmentStatus.PendingPayment).OrderBy(isn => isn.DueDate))
            .Where(c => c.Enrollment.SchoolStudentId == accountId && chargeIds.Contains(c.Id))
            .OrderBy(c => c.Enrollment.Course.StartDate)
            .ToListAsync(cancellationToken);

        var educationAccount = await _accountRepository.Query(tracking: false)
            .Include(a => a.Citizen)
            .FirstOrDefaultAsync(a => a.SchoolStudent!.EducationAccountId == accountId, cancellationToken);

        ValidatePaymentRequest(chargePaymentRequestInfors, charges, educationAccount, request.CreditBalanceApplied);

        var paymentExisting = await ValidateExistingStripePaymentAsync(educationAccount!.Id, chargeIds, sessionService, cancellationToken);
        if (paymentExisting != null) return paymentExisting;

        decimal totalOwed = 0.0m;
        var billingItems = new List<BillingItem>();
        //Tạo các khoản thu để tính toán trừ tiền credit balance (nếu có) và tính tổng nợ cần trả trong đợt thanh toán này
        totalOwed = CalculateTotalOwedAndPopulateBillingItems(chargePaymentRequestInfors, charges, totalOwed, billingItems);

        decimal balanceToApply = Math.Min(request.CreditBalanceApplied, totalOwed);
        decimal remainingToPayViaStripe = totalOwed - balanceToApply;

        // Tiến hành tạo Payments (chờ xử lý) và các PaymentAllocation tương ứng
        var payments = await CreatePrePaymentAndPaymentAllocationsAsync(
            chargePaymentRequestInfors, charges, billingItems, educationAccount,
            totalOwed, balanceToApply, remainingToPayViaStripe, cancellationToken);

        var stripePayment = payments.FirstOrDefault(p => p.PaymentMethod == PaymentMethod.OnlinePayment);
        var walletPayment = payments.FirstOrDefault(p => p.PaymentMethod == PaymentMethod.EducationBalance);

        // Trường hợp số dư ví (Wallet) đủ cover toàn bộ tiền cần thanh toán
        // không cần gọi Stripe, trực tiếp process payment với trạng thái thành công (Succeeded).
        if (remainingToPayViaStripe <= 0 && walletPayment != null)
        {
            await ProcessPaymentInternalAsync(
                [walletPayment.Id], accountId, null,
                targetStatus: PaymentStatus.Succeeded, cancellationToken);

            return new PaymentSessionResponseDTO
            {
                Link = "",
                Status = PaymentStatus.Succeeded.ToString()
            };
        }

        //Ngược lại thực hiện tạo thanh toán qua stripe (Online Payment)
        if (stripePayment == null)
            throw new InternalAppException($"Failed to generate stripe {nameof(Payment)} record.");

        // Chuẩn bị LineItems (danh sách món hàng) gửi lên Stripe Checkout
        var lineItems = new List<SessionLineItemOptions>();
        CreateItemsCheckOut(billingItems, balanceToApply, lineItems);

        //Tạo metadata sẽ đc stripe phàn hồi về webhook để handel đúng Payments đã tạo
        var sessionMetadataDto = new StripeSessionMetadataDTO
        {
            AccountId = accountId,
            PaymentIds = payments.Select(p => p.Id).ToList()
        };

        //Tạo config cơ bản cho stripe
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { _configuration.StripeConfig.Method },
            LineItems = lineItems,
            Mode = _configuration.StripeConfig.Mode,
            SuccessUrl = _configuration.StripeConfig.SuccessUrl,
            CancelUrl = _configuration.StripeConfig.CancelUrl,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_configuration.StripeConfig.SessionExpiryMinutes),
            CustomerEmail = educationAccount.Citizen.Email ?? "",
            Metadata = new Dictionary<string, string>
            {
                { "sessionData", JsonSerializer.Serialize(sessionMetadataDto) }
            }
        };

        var session = await sessionService.CreateAsync(options, cancellationToken: cancellationToken);
        //Gắn external reference cho payments để kiểm tra trong trường hợp người dùng spam tạo link cho cùng 1 list charges
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


            await _unitOfWork.SaveChangeAsync(token);
        }, cancellationToken);

        return new PaymentSessionResponseDTO
        {
            Link = session.Url,
            Status = PaymentStatus.Pending.ToString()
        };
    }

    private static decimal CalculateTotalOwedAndPopulateBillingItems(
        List<ChargePaymentRequestInfor> chargePaymentRequestInfors,
        List<Models.Charge> charges, decimal totalOwed, List<BillingItem> billingItems)
    {
        foreach (var charge in charges)
        {
            var requestInfo = chargePaymentRequestInfors.First(ci => ci.ChargeId == charge.Id);

            switch (requestInfo.Intent)
            {
                case DTOs.Payments.PaymentIntent.PayFull:
                    billingItems.Add(new BillingItem(charge, null, charge.RemainingAmount));
                    totalOwed += charge.RemainingAmount;
                    break;
                case DTOs.Payments.PaymentIntent.CreateInstallment:
                    decimal amountToPay = Math.Round(charge.RemainingAmount / requestInfo.InstallmentNumber!.Value, 2, MidpointRounding.AwayFromZero);
                    billingItems.Add(new BillingItem(charge, null, amountToPay));
                    totalOwed += amountToPay;
                    break;
                case DTOs.Payments.PaymentIntent.PayCurrentInstallment:
                    var firstPending = charge.Installments.First();
                    billingItems.Add(new BillingItem(charge, firstPending.Id, firstPending.Amount));
                    totalOwed += firstPending.Amount;
                    break;
                case DTOs.Payments.PaymentIntent.PayRemainingInstallments:
                    foreach (var inst in charge.Installments)
                    {
                        billingItems.Add(new BillingItem(charge, inst.Id, inst.Amount));
                        totalOwed += inst.Amount;
                    }
                    break;
            }
        }

        return totalOwed;
    }

    /// <summary>
    /// Chuẩn bị danh sách LineItem (Sản phẩm/Khóa học) để hiển thị trên giao diện thanh toán của Stripe.
    /// Phân bổ số dư ví (Credit Balance) vào từng khóa học để tính ra số tiền cuối cùng Stripe cần thu.
    /// </summary>
    private static void CreateItemsCheckOut(List<BillingItem> billingItems, decimal accountCreditBalance, List<SessionLineItemOptions> lineItems)
    {
        var chargeGroups = billingItems.GroupBy(b => b.Charge);
        foreach (var group in chargeGroups)
        {
            var charge = group.Key;
            var enrollment = charge.Enrollment;
            decimal chargeAmountToPay = group.Sum(b => b.AmountToPay);

            decimal coveredByCreditBalance = Math.Min(chargeAmountToPay, accountCreditBalance);
            decimal amountToPayViaStripe = chargeAmountToPay - coveredByCreditBalance;

            accountCreditBalance -= coveredByCreditBalance;

            lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "sgd",
                    UnitAmount = (long)(amountToPayViaStripe * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = enrollment.CourseNameSnapshot,
                        Description = coveredByCreditBalance > 0
                            ? $"• Remaining Owed: {charge.RemainingAmount:C} SGD | • Current Payment Portion: {chargeAmountToPay:C} SGD | " +
                              $"• Credit Balance Applied: -{coveredByCreditBalance:C} SGD"
                            : $"• Remaining Owed: {charge.RemainingAmount:C} SGD | • Payment Portion: {chargeAmountToPay:C} SGD"
                    }
                },
                Quantity = 1
            });
        }
    }

    /// <summary>
    /// Lưu nháp (Pending) các bản ghi Payment và PaymentAllocation vào Database trước khi gọi Stripe API.
    /// Sử dụng logic Waterfall: Ưu tiên rút cạn số dư Ví (Wallet) trước, phần dư mới đẩy sang Stripe.
    /// </summary>
    private async Task<List<Payment>> CreatePrePaymentAndPaymentAllocationsAsync(
        List<ChargePaymentRequestInfor> requestInfos, List<Models.Charge> charges,
        List<BillingItem> billingItems, EducationAccount educationAccount,
        decimal totalOwed, decimal balanceToApply,
        decimal remainingToPayViaStripe, CancellationToken cancellationToken)
    {
        var payments = new List<Payment>();

        await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
        {
            Payment? walletPayment = null;
            Payment? stripePayment = null;

            if (balanceToApply > 0)
            {
                walletPayment = new Payment
                {
                    Status = PaymentStatus.Pending,
                    PaymentMethod = PaymentMethod.EducationBalance,
                    TotalAmount = balanceToApply,
                    AccountNumberSnapshot = educationAccount.AccountNumber,
                    CitizenNricSnapshot = charges.First().Enrollment.CitizenNricSnapshot,
                    CitizenFullNameSnapshot = charges.First().Enrollment.CitizenFullNameSnapshot
                };
                walletPayment.TryValidate();
                await _paymentRepository.AddAsync(walletPayment, token);
                payments.Add(walletPayment);
            }

            if (remainingToPayViaStripe > 0)
            {
                stripePayment = new Payment
                {
                    Status = PaymentStatus.Pending,
                    PaymentMethod = PaymentMethod.OnlinePayment,
                    TotalAmount = remainingToPayViaStripe,
                    AccountNumberSnapshot = educationAccount.AccountNumber,
                    CitizenNricSnapshot = charges.First().Enrollment.CitizenNricSnapshot,
                    CitizenFullNameSnapshot = charges.First().Enrollment.CitizenFullNameSnapshot
                };
                stripePayment.TryValidate();
                await _paymentRepository.AddAsync(stripePayment, token);
                payments.Add(stripePayment);
            }

            await _unitOfWork.SaveChangeAsync(token);

            decimal currentWalletBalance = balanceToApply;

            // Lặp qua từng khoản thu (BillingItem) để trích tiền Wallet và Stripe
            // Logic Waterfall: Rút cạn Wallet trước, phần dư đẩy sang Stripe
            foreach (var item in billingItems)
            {
                var charge = item.Charge;
                decimal amountToPay = item.AmountToPay;
                decimal coveredByWallet = Math.Min(amountToPay, currentWalletBalance);
                decimal coveredByStripe = amountToPay - coveredByWallet;

                currentWalletBalance -= coveredByWallet;

                if (coveredByWallet > 0 && walletPayment != null)
                {
                    var allocation = new PaymentAllocation
                    {
                        PaymentId = walletPayment.Id,
                        ChargeId = charge.Id,
                        ChargeInstallmentId = item.TargetInstallmentId,
                        CourseNameSnapshot = charge.Enrollment.CourseNameSnapshot,
                        SchoolNameSnapshot = charge.Enrollment.SchoolNameSnapshot,
                        ChargeGrossAmountSnapshot = charge.GrossAmount,
                        ChargeNetAmountSnapshot = charge.NetAmount,
                        ChargeRemainingAmountSnapshot = charge.RemainingAmount,
                        Amount = coveredByWallet
                    };
                    allocation.TryValidate();
                    await _paymentAllocationRepository.AddAsync(allocation, token);

                }

                // Nếu Stripe phải gánh khoản này, tạo Allocation tương ứng
                if (coveredByStripe > 0 && stripePayment != null)
                {
                    var allocation = new PaymentAllocation
                    {
                        PaymentId = stripePayment.Id,
                        ChargeId = charge.Id,
                        ChargeInstallmentId = item.TargetInstallmentId,
                        CourseNameSnapshot = charge.Enrollment.CourseNameSnapshot,
                        SchoolNameSnapshot = charge.Enrollment.SchoolNameSnapshot,
                        ChargeGrossAmountSnapshot = charge.GrossAmount,
                        ChargeNetAmountSnapshot = charge.NetAmount,
                        ChargeRemainingAmountSnapshot = charge.RemainingAmount,
                        Amount = coveredByStripe
                    };
                    allocation.TryValidate();
                    await _paymentAllocationRepository.AddAsync(allocation, token);

                }
            }

            foreach (var p in payments)
            {
                var actionMsg = $"{nameof(Payment)} {p.Id} Init for paying {nameof(Course)}s {nameof(Models.Charge)} - Total Owed: {totalOwed}, Credit Applied: {balanceToApply}";
                if (actionMsg.Length > 90) actionMsg = actionMsg.Substring(0, 90) + "...";
                await LogAuditAsync(AuditLogCategory.Billing, actionMsg, educationAccount.Id, educationAccount.Citizen.Nric, token);
            }


            await _unitOfWork.SaveChangeAsync(token);
        }, cancellationToken);

        return payments;
    }

    /// <summary>
    /// Đảm bảo không có khoản thu (Charge) nào đang bị "khóa" (Locked) bởi một giao dịch đang Pending khác.
    /// Query trực tiếp vào PaymentAllocation để bắt được cả giao dịch bằng Ví lẫn Stripe.
    /// </summary>
    private async Task<PaymentSessionResponseDTO?> ValidateExistingStripePaymentAsync(int accountId, List<int> requestedChargeIds, SessionService sessionService, CancellationToken cancellationToken)
    {
        var overlappingAllocations = await _paymentAllocationRepository.Query(tracking: false)
            .Include(pa => pa.Payment)
            .Where(pa => pa.Payment.Status == PaymentStatus.Pending
                      && pa.Payment.ExternalReference != null
                      && requestedChargeIds.Contains(pa.ChargeId))
            .ToListAsync(cancellationToken);

        if (overlappingAllocations.Count > 0)
        {
            // Lấy ra danh sách các Stripe Session (ExternalReference) đang khóa các Charge này
            // Loại bỏ hậu tố "_wallet" nếu có để gom nhóm chính xác
            var sessionIds = overlappingAllocations
                .Select(pa => pa.Payment.ExternalReference!)
                .Select(s => s.EndsWith("_wallet") ? s.Substring(0, s.Length - 7) : s)
                .Distinct()
                .ToList();

            // Nếu chỉ có 1 session duy nhất bị trùng, kiểm tra xem nó có khớp 100% với giỏ hàng hiện tại không
            if (sessionIds.Count == 1)
            {
                var sessionId = sessionIds.First();
                var walletSessionId = sessionId + "_wallet";

                // Lấy tất cả các ChargeId nằm trong session này (bao gồm cả Stripe và Wallet)
                var sessionChargeIds = await _paymentAllocationRepository.Query(tracking: false)
                    .Include(pa => pa.Payment)
                    .Where(pa => pa.Payment.ExternalReference == sessionId || pa.Payment.ExternalReference == walletSessionId)
                    .Select(pa => pa.ChargeId)
                    .Distinct()
                    .ToListAsync(cancellationToken);

                bool isExactMatch = sessionChargeIds.Count == requestedChargeIds.Count
                                    && !sessionChargeIds.Except(requestedChargeIds).Any();

                if (isExactMatch)
                {
                    // Khớp 100% giỏ hàng, ta kiểm tra Stripe Session xem còn hạn thanh toán không
                    var existingSession = await sessionService.GetAsync(sessionId, cancellationToken: cancellationToken);
                    if (existingSession.Status == "open" && existingSession.PaymentStatus == "unpaid")
                    {
                        return new PaymentSessionResponseDTO
                        {
                            Link = existingSession.Url,
                            Status = PaymentStatus.Pending.ToString(),
                        };
                    }
                }
            }

            // Nếu trùng một phần (ví dụ: đã gom 3 khoá vào 1 session, giờ lại thanh toán lẻ 1 khoá)
            // hoặc session đã hết hạn / lỗi, ta sẽ block lại yêu cầu.
            throw new ValidationFailureException(new Dictionary<string, string>
            {
                { "ChargeIds", $"One or more selected {nameof(Course)}s are already locked in another pending {nameof(Payment)} session. Please wait for it to complete or expire." }
            });
        }
        return null;
    }

    /// <summary>
    /// Kiểm tra tính hợp lệ của Request theo các luật Business (Ví dụ: Không tạo 2 luồng trả góp, trả đúng kỳ, v.v.)
    /// </summary>
    private static void ValidatePaymentRequest(List<ChargePaymentRequestInfor> requestInfos, List<Models.Charge> charges, EducationAccount? educationAccount, decimal creditBalanceApplied)
    {
        var errors = new Dictionary<string, string>();

        if (educationAccount == null) throw new InternalAppException("Current login user not found!");
        if (educationAccount.Status != EducationAccountStatus.Active)
            errors[$"{nameof(EducationAccount)}"] = $"User does not have an active {nameof(EducationAccount)}";
        if (creditBalanceApplied > educationAccount.EducationCreditBalance)
            errors["CreditBalanceApplied"] = "Applied balance exceeds available credit balance.";

        var chargeIds = requestInfos.Select(r => r.ChargeId).ToList();
        if (chargeIds.Count == 0)
            errors[$"{nameof(Models.Charge)}Ids"] = $"At least one {nameof(Models.Charge)} must be selected for payment.";

        var distinctRequestedIds = chargeIds.Distinct().ToList();
        if (charges.Count != distinctRequestedIds.Count)
        {
            var foundIds = charges.Select(c => c.Id).ToList();
            var missingIds = distinctRequestedIds.Except(foundIds);
            errors[$"{nameof(Models.Charge)}Ids"] = $"{nameof(Models.Charge)} ID(s) not found or not belonging to user: {string.Join(", ", missingIds)}.";
        }

        foreach (var charge in charges)
        {
            var enrollment = charge.Enrollment;
            var info = requestInfos.First(ci => ci.ChargeId == charge.Id);

            if (enrollment.Course.Status == CourseStatus.Draft)
                errors[$"{nameof(Course)}_{enrollment.CourseId}"] = $"{nameof(Course)} '{enrollment.CourseNameSnapshot}' is not open for billing.";
            if (charge.Status == ChargeStatus.Paid || charge.RemainingAmount <= 0)
                errors[$"{nameof(Models.Charge)}_{charge.Id}"] = $"The {nameof(Models.Charge)} for '{enrollment.CourseNameSnapshot}' has already been fully paid.";

            bool hasPendingInstallments = charge.Installments.Any();

            switch (info.Intent)
            {
                case DTOs.Payments.PaymentIntent.PayFull:
                    if (hasPendingInstallments)
                        errors[$"{nameof(Models.Charge)}_{charge.Id}"] = "Cannot PayFull while an installment plan is active. Use PayRemainingInstallments instead.";
                    break;
                case DTOs.Payments.PaymentIntent.CreateInstallment:
                    if (hasPendingInstallments)
                        errors[$"{nameof(Models.Charge)}_{charge.Id}"] = $"Cannot create new {nameof(ChargeInstallment)} plan. An {nameof(ChargeInstallment)} plan already exists.";
                    if (!info.InstallmentNumber.HasValue)
                        errors[$"{nameof(Models.Charge)}_{charge.Id}"] = $"Installment number is required to create an {nameof(ChargeInstallment)} plan.";
                    break;
                case DTOs.Payments.PaymentIntent.PayCurrentInstallment:
                    if (!hasPendingInstallments)
                        errors[$"{nameof(Models.Charge)}_{charge.Id}"] = $"Cannot pay current {nameof(ChargeInstallment)} because no active {nameof(ChargeInstallment)} plan exists.";
                    if (info.InstallmentNumber.HasValue)
                        errors[$"{nameof(Models.Charge)}_{charge.Id}"] = "Pay current installment does not required installment number";
                    break;
                case DTOs.Payments.PaymentIntent.PayRemainingInstallments:
                    if (!hasPendingInstallments)
                        errors[$"{nameof(Models.Charge)}_{charge.Id}"] = $"No active {nameof(ChargeInstallment)} plan exists to pay off remaining installments. Or remaining {nameof(ChargeInstallment)} already paid";
                    if (info.InstallmentNumber.HasValue)
                        errors[$"{nameof(Models.Charge)}_{charge.Id}"] = $"Pay all {nameof(ChargeInstallment)} does not required installment number";
                    break;
            }
        }

        if (errors.Count != 0) throw new ValidationFailureException(errors);
    }

    /// <summary>
    /// Webhook đích: Lắng nghe sự kiện từ Stripe trả về để xử lý cập nhật trạng thái đơn hàng.
    /// </summary>
    public async Task HandleWebhookAsync(string payload, string stripeSignature)
    {
        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                payload, stripeSignature, _configuration.StripeConfig.WebhookSecret);
        }
        catch (StripeException)
        {
            throw new ValidationFailureException(new Dictionary<string, string>
            { { "Webhook", "Invalid Stripe webhook signature." } });
        }

        switch (stripeEvent.Type)
        {
            case "checkout.session.completed":
                if (stripeEvent.Data.Object is Session completedSession)
                    await ProcessStripeSessionAsync(null, completedSession, PaymentStatus.Succeeded, CancellationToken.None);
                break;
            case "checkout.session.expired":
                if (stripeEvent.Data.Object is Session expiredSession)
                    await ProcessStripeSessionAsync(null, expiredSession, PaymentStatus.Failed, CancellationToken.None);
                break;
        }
    }

    /// <summary>
    /// Giải mã Metadata từ Session của Stripe và gọi hàm xử lý thanh toán nội bộ.
    /// </summary>
    private async Task ProcessStripeSessionAsync(int? accountId, Session session, PaymentStatus targetStatus, CancellationToken cancellationToken)
    {
        var metadata = session.Metadata;
        if (metadata == null || !metadata.ContainsKey("sessionData")) throw new DataNotFoundException($"Session not found for sessionId {session.Id}");

        var sessionData = JsonSerializer.Deserialize<StripeSessionMetadataDTO>(metadata["sessionData"]);
        if (sessionData == null) throw new DataNotFoundException($"Session data not found for sessionId {session.Id}");
        if (accountId != null && sessionData.AccountId != accountId) throw new DataConflictException($"Current User not belong to payment Session {session.Id}");

        await ProcessPaymentInternalAsync(sessionData.PaymentIds, sessionData.AccountId, session.Id, targetStatus, cancellationToken);
    }

    /// <summary>
    /// Hàm lõi xử lý thanh toán (Dùng chung cho cả Wallet và Stripe).
    /// Giao dịch nguyên tử (Atomic): Cập nhật trạng thái Payment, trừ nợ Charge, sinh/trừ kỳ trả góp, và ghi Log.
    /// </summary>
    private async Task ProcessPaymentInternalAsync(
        List<int> paymentIds, int accountId, string? externalReference,
        PaymentStatus targetStatus, CancellationToken cancellationToken)
    {
        try
        {
            // BƯỚC 1: LẤY VÀ KIỂM TRA DỮ LIỆU THANH TOÁN
            // Query lấy danh sách các Payment dựa trên danh sách ID (bao gồm cả Wallet và Stripe nếu có)
            var relatedPayments = await _paymentRepository.Query(tracking: true)
                .Include(p => p.PaymentAllocations).ThenInclude(pa => pa.Charge).ThenInclude(c => c.Installments)
                .Where(p => paymentIds.Contains(p.Id))
                .ToListAsync(cancellationToken);

            if (relatedPayments.Count == 0) throw new InternalAppException($"Invalid {nameof(Payment)}s data.");

            // Lọc ra các payment thực sự đang Pending
            relatedPayments = relatedPayments.Where(p => p.Status == PaymentStatus.Pending).ToList();
            if (relatedPayments.Count == 0) return;

            var educationAccount = await _accountRepository.Query(tracking: true)
                .Include(a => a.Citizen)
                .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);
            if (educationAccount == null) throw new InternalAppException("Invalid education account data.");

            bool isSuccess = targetStatus == PaymentStatus.Succeeded;
            decimal totalPaid = 0m;
            decimal totalCreditBalanceCovered = 0m;
            decimal remainingPaymentViaStripe = 0m;

            // BƯỚC 2: THỰC THI GIAO DỊCH (TRANSACTION)
            // Đảm bảo tính nguyên vẹn dữ liệu: Cập nhật Payment, cấn trừ Charge, sinh Installment và ghi Log phải đồng thời thành công
            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                //Cập nhập trạng thái của Payment khi trả về từ stripe (Succeeded, Failed, Canceled),
                //nếu thanh toán trực tiếp qua ví trạng thái sẽ luôn Successed trừ khi lỗi (nội bộ) throw ở exception thì payment sẽ luôn ở pending
                UpdatePaymentStatus(externalReference, targetStatus, relatedPayments, isSuccess, ref totalCreditBalanceCovered, ref remainingPaymentViaStripe);

                //Nếu trạng thái thanh toán là thành công thực hiện cập nhập/tạo trạng thái công nợ
                if (isSuccess) totalPaid = await ProcessChargeInstallmentsAsync(relatedPayments, totalPaid, token);

                //Nếu trạng thái thanh toán là thành công (Succeeded) thực hiện tạo Transaction cho mỗi payment tương ứng 
                //Với các trạng thái khác chỉ lưu payment và payment allocation với status tương ứng không tạo hay cập nhập (Charge, Installment, Credit Balance)
                if (isSuccess && (totalCreditBalanceCovered > 0 || remainingPaymentViaStripe > 0))
                    await CreateEducationCreditTransactionsAsync(relatedPayments, educationAccount, totalCreditBalanceCovered, remainingPaymentViaStripe, token);


                //Log audit thông tin của giao dịch
                var pIdStr = string.Join(",", paymentIds);
                var actionMsg = $"{nameof(Payment)}s finished with status {targetStatus} - for PaymentIds: {pIdStr}";
                if (actionMsg.Length > 90) actionMsg = actionMsg.Substring(0, 90) + "...";
                await LogAuditAsync(AuditLogCategory.Billing, actionMsg, accountId, educationAccount.Citizen.Nric, token);

                //Gửi mail về thông báo giao dịch
                await SendPaymentEmailNotificationAsync(targetStatus, relatedPayments.First(), educationAccount, isSuccess, totalPaid, totalCreditBalanceCovered, remainingPaymentViaStripe);
                await _unitOfWork.SaveChangeAsync(token);
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InternalAppException("An error occurred while processing the payment.", ex);
        }
    }

    private async Task CreateEducationCreditTransactionsAsync(
        List<Payment> relatedPayments, EducationAccount educationAccount,
        decimal totalCreditBalanceCovered, decimal remainingPaymentViaStripe, CancellationToken token)
    {
        var balanceBefore = educationAccount.EducationCreditBalance;

        if (totalCreditBalanceCovered > 0 && balanceBefore < totalCreditBalanceCovered)
            throw new ValidationFailureException(new Dictionary<string, string>
                           { { "EducationCreditBalance", "Insufficient credit balance at payment finalization." }});

        var balanceAfter = balanceBefore - totalCreditBalanceCovered;
        educationAccount.EducationCreditBalance = balanceAfter;

        // Ghi nhận log giao dịch vào ví
        // Đối với thanh toán qua Ví (Wallet), direction là Debit (Trừ tiền ví)
        if (totalCreditBalanceCovered > 0)
        {
            var coursesNames = string.Join(", ", relatedPayments
                .Where(p => p.PaymentMethod == PaymentMethod.EducationBalance)
                .SelectMany(p => p.PaymentAllocations)
                .Select(pa => pa.CourseNameSnapshot).Distinct());

            var description = $"Payment for Courses: {coursesNames}. fee covered via credit balance";

            var transactionDebit = new EducationCreditTransaction
            {
                Type = EducationCreditTransactionType.CourseFeePayment,
                Direction = EducationCreditTransactionDirection.Debit,
                Amount = totalCreditBalanceCovered,
                BalanceBefore = balanceBefore,
                BalanceAfter = balanceAfter,
                Description = description,
                EducationAccountId = educationAccount.Id,
                Payment = relatedPayments.FirstOrDefault(p => p.PaymentMethod == PaymentMethod.EducationBalance)
            };
            transactionDebit.TryValidate();
            await _transactionRepository.AddAsync(transactionDebit, token);
        }

        // Đối với thanh toán Online (Stripe), direction là Neutral (Không ảnh hưởng số dư ví thực)
        if (remainingPaymentViaStripe > 0)
        {

            var coursesNames = string.Join(", ", relatedPayments
               .Where(p => p.PaymentMethod == PaymentMethod.OnlinePayment)
               .SelectMany(p => p.PaymentAllocations)
               .Select(pa => pa.CourseNameSnapshot).Distinct());

            var description = $"Payment for Courses: {coursesNames}. fee covered via online card, Credit balance keep remaining";

            var transactionNeutral = new EducationCreditTransaction
            {
                Type = EducationCreditTransactionType.CourseFeePayment,
                Direction = EducationCreditTransactionDirection.Neutral,
                Amount = remainingPaymentViaStripe,
                BalanceBefore = balanceAfter,
                BalanceAfter = balanceAfter,
                Description = description,
                EducationAccountId = educationAccount.Id,
                Payment = relatedPayments.FirstOrDefault(p => p.PaymentMethod == PaymentMethod.OnlinePayment)
            };
            transactionNeutral.TryValidate();
            await _transactionRepository.AddAsync(transactionNeutral, token);
        }
    }

    private async Task<decimal> ProcessChargeInstallmentsAsync(List<Payment> relatedPayments, decimal totalPaid, CancellationToken token)
    {
        // 2.2 Gom nhóm Allocation theo từng Charge để xử lý công nợ
        var allAllocations = relatedPayments.SelectMany(p => p.PaymentAllocations).ToList();
        var groupedByCharge = allAllocations.GroupBy(pa => pa.Charge);

        foreach (var group in groupedByCharge)
        {
            var charge = group.Key;
            // Tổng số tiền đã trả cho Charge này trong lần giao dịch hiện tại (từ cả Wallet lẫn Stripe)
            decimal paidForCharge = group.Sum(a => a.Amount);
            totalPaid += paidForCharge;

            charge.PaidAmount += paidForCharge;
            charge.RemainingAmount -= paidForCharge;

            var targetInstallmentIds = group
                .Where(a => a.ChargeInstallmentId != null)
                .Select(a => a.ChargeInstallmentId!.Value)
                .Distinct()
                .ToList();

            // KỊCH BẢN A: Thanh toán cho các kỳ trả góp cụ thể (1 hoặc nhiều kỳ)
            if (targetInstallmentIds.Any())
            {
                foreach (var id in targetInstallmentIds)
                {
                    var installment = charge.Installments.FirstOrDefault(i => i.Id == id);
                    if (installment != null)
                    {
                        installment.Status = ChargeInstallmentStatus.Paid;
                    }
                }

                // Fallback: Nếu PayRemainingInstallments vô tình thanh toán cạn nợ, ta đánh dấu các kỳ còn lại thành Paid
                if (charge.RemainingAmount <= 0)
                {
                    foreach (var inst in charge.Installments.Where(i => i.Status == ChargeInstallmentStatus.PendingPayment))
                    {
                        inst.Status = ChargeInstallmentStatus.Paid;
                    }
                }
            }
            // KỊCH BẢN B: Thanh toán nốt toàn bộ nợ (PayFull)
            else if (charge.RemainingAmount <= 0)
            {
                foreach (var inst in charge.Installments.Where(i => i.Status == ChargeInstallmentStatus.PendingPayment))
                {
                    inst.Status = ChargeInstallmentStatus.Paid;
                }
            }
            // KỊCH BẢN C: Lần đầu tiên đăng ký trả góp (CreateInstallment)
            else
            {
                // Logic tạo mới luồng trả góp
                // Số tháng trả góp = tổng số tiền nợ / số tiền đóng kỳ 1
                int totalInstallments = (int)Math.Round((charge.RemainingAmount + paidForCharge) / paidForCharge);
                if (totalInstallments > 1)
                {
                    charge.PaymentPlanMonths = totalInstallments;
                    var now = DateTime.UtcNow;

                    // Kỳ 1: Trả ngay lập tức -> Đánh dấu Paid
                    var firstInst = new ChargeInstallment
                    {
                        ChargeId = charge.Id,
                        InstallmentNumber = 1,
                        Status = ChargeInstallmentStatus.Paid,
                        DueDate = now,
                        Amount = paidForCharge
                    };
                    firstInst.TryValidate();
                    await _installmentRepository.AddAsync(firstInst, token);

                    foreach (var allocation in group)
                    {
                        allocation.ChargeInstallment = firstInst;
                    }

                    // Các kỳ tiếp theo -> Đánh dấu Pending
                    decimal totalOriginalAmount = charge.RemainingAmount + paidForCharge;
                    decimal baseAmount = paidForCharge;
                    decimal amountAllocatedSoFar = paidForCharge;

                    for (int i = 2; i <= totalInstallments; i++)
                    {
                        decimal nextAmount;
                        if (i == totalInstallments)
                        {
                            nextAmount = totalOriginalAmount - amountAllocatedSoFar;
                        }
                        else
                        {
                            nextAmount = baseAmount;
                        }

                        amountAllocatedSoFar += nextAmount;

                        var nextInst = new ChargeInstallment
                        {
                            ChargeId = charge.Id,
                            InstallmentNumber = i,
                            Status = ChargeInstallmentStatus.PendingPayment,
                            DueDate = now.AddMonths(i - 1),
                            Amount = nextAmount
                        };
                        nextInst.TryValidate();
                        await _installmentRepository.AddAsync(nextInst, token);
                    }
                }
            }

            if (charge.RemainingAmount <= 0)
            {
                charge.RemainingAmount = 0;
                charge.Status = ChargeStatus.Paid;
            }
        }

        return totalPaid;
    }

    // 2.1 Cập nhật trạng thái cho tất cả các Payment liên quan lưu lại số tiền chi trả qua ví hoặc stripe (Online payment)
    private static void UpdatePaymentStatus(
        string? externalReference, PaymentStatus targetStatus, List<Payment> relatedPayments,
        bool isSuccess, ref decimal totalCreditBalanceCovered, ref decimal remainingPaymentViaStripe)
    {
        var paidAt = DateTime.UtcNow;

        foreach (var payment in relatedPayments)
        {
            payment.Status = targetStatus;
            if (payment.PaymentMethod == PaymentMethod.OnlinePayment)
            {
                payment.ExternalReference = externalReference ?? payment.ExternalReference;
            }
            if (isSuccess) payment.PaidAt = paidAt;

            if (isSuccess && payment.PaymentMethod == PaymentMethod.EducationBalance)
                totalCreditBalanceCovered += payment.TotalAmount;
            if (isSuccess && payment.PaymentMethod == PaymentMethod.OnlinePayment)
                remainingPaymentViaStripe += payment.TotalAmount;
        }
    }

    /// <summary>
    /// Gửi Email hóa đơn/biên lai cho học viên sau khi giao dịch thành công.
    /// </summary>
    private async Task SendPaymentEmailNotificationAsync(PaymentStatus targetStatus, Payment payment, EducationAccount educationAccount, bool isSuccess, decimal totalPaid, decimal totalWalletCovered, decimal totalStripeCovered)
    {
        var subject = isSuccess ? "Payment Confirmed - MOS" : $"Payment {targetStatus} - MOS";
        var subMessage = isSuccess
        ? $"Deduction from your credit balance: {totalWalletCovered:C} SGD and deduction via online payment: {totalStripeCovered:C} SGD"
        : "Contact MOS Staff for more information!";
        var message = isSuccess
            ? $"Dear {payment.CitizenFullNameSnapshot}, your payment of {totalPaid:C} SGD has been confirmed.\n" + subMessage
            : $"Dear {payment.CitizenFullNameSnapshot}, your payment session has been {targetStatus}.\n" + subMessage;

        await _outboxWriter.EnqueueEmailAsync(
            educationAccount.Citizen.Email!,
            new EmailTemplate(Subject: subject, HtmlBody: message, TextBody: message)
        );
    }

    public async Task<PaymentSessionResponseDTO> HandleSessionSuccessAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var accountId = _currentUserService.UserId;
        var educationAccount = await _accountRepository.Query(tracking: false)
            .FirstOrDefaultAsync(a => a.SchoolStudent!.EducationAccountId == accountId, cancellationToken);

        if (educationAccount == null) throw new InternalAppException("Current account holder not found!");

        var payments = await _paymentRepository
            .Query(tracking: false)
            .Where(p =>
             (p.ExternalReference == sessionId || p.ExternalReference == sessionId + "_wallet") &&
              p.AccountNumberSnapshot == educationAccount.AccountNumber)
            .ToListAsync();


        if (payments == null || !payments.Any()) throw new DataNotFoundException($"Session not found or invalid {nameof(Payment)}s count for sessionId {sessionId}");

        return new PaymentSessionResponseDTO
        {
            Link = "",
            Status = payments[0].Status.ToString()
        };
    }

    public async Task<PaymentSessionResponseDTO> HandleSessionCancelledAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var sessionService = new SessionService();
        var session = await sessionService.GetAsync(sessionId, cancellationToken: cancellationToken);
        if (session == null) throw new DataNotFoundException($"Session not found for sessionId {sessionId}");

        var accountId = _currentUserService.UserId;
        var educationAccount = await _accountRepository.Query(tracking: false)
          .FirstOrDefaultAsync(a => a.SchoolStudent!.EducationAccountId == accountId, cancellationToken);

        if (educationAccount == null) throw new InternalAppException("Current account holder not found!");

        await ProcessStripeSessionAsync(educationAccount.Id, session, PaymentStatus.Canceled, cancellationToken);

        return new PaymentSessionResponseDTO
        {
            Link = "",
            Status = PaymentStatus.Canceled.ToString()
        };
    }

    /// <summary>
    /// Ghi nhận lịch sử (Audit Log) cho các hành động quan trọng (giới hạn Action dưới 100 ký tự).
    /// </summary>
    private async Task LogAuditAsync(AuditLogCategory category, string action, int accountId, string? nric, CancellationToken cancellationToken)
    {
        await _auditLogWriter.LogAsync(category, action, nric, cancellationToken);
    }
}