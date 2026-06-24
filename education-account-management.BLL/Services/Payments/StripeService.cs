using BLL.Interfaces.Payments;
using DTOs.Payments;
using Interfaces.Audit;
using Interfaces.Email;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Text.Json;

namespace BLL.Services.Payments;

public class StripeService(
    IOptions<StripeSettings> stripeSettings,
    IUnitOfWork unitOfWork,
    IOutboxWriter outboxWriter,
    IEmailService emailService,
    ICurrentUserService currentUserService,
    IAuditLogWriter auditLogWriter) : IStripeService
{
    private readonly IOptions<StripeSettings> _stripeSettings = stripeSettings;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOutboxWriter _outboxWriter = outboxWriter;
    private readonly IEmailService _emailService = emailService;
    private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    private readonly IGenericRepository<Enrollment> _enrollmentRepository = unitOfWork.Repository<Enrollment>();
    private readonly IGenericRepository<EducationAccount> _accountRepository = unitOfWork.Repository<EducationAccount>();
    private readonly IGenericRepository<Models.Charge> _chargeRepository = unitOfWork.Repository<Models.Charge>();
    private readonly IGenericRepository<Payment> _paymentRepository = unitOfWork.Repository<Payment>();
    private readonly IGenericRepository<PaymentAllocation> _paymentAllocationRepository = unitOfWork.Repository<PaymentAllocation>();
    private readonly IGenericRepository<EducationCreditTransaction> _transactionRepository = unitOfWork.Repository<EducationCreditTransaction>();

    public async Task<PaymentSessionResponseDTO> CreateCheckoutSessionAsync(
        List<int> courseIds,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(courseIds);
        if (courseIds.Count == 0)
            throw new ValidationFailureException(new Dictionary<string, string>
            { { "CourseIds", "At least one course must be selected for payment." } });

        var sessionService = new SessionService();

        var paymentExisting = await ValidateExistingPayment(courseIds, sessionService, cancellationToken);
        if (paymentExisting != null) return paymentExisting;

        var accountId = _currentUserService.UserId;

        var enrollments = await _enrollmentRepository.Query(tracking: false)
            .Include(e => e.Course)
            .Include(e => e.Charge)
            .Where(e => e.SchoolStudentId == accountId && courseIds.Contains(e.CourseId))
            .OrderBy(e => e.Course.StartDate)
            .ToListAsync(cancellationToken);

        var educationAccount = await _accountRepository.Query(tracking: false)
            .Include(a => a.Citizen)
            .FirstOrDefaultAsync(a => a.SchoolStudent!.EducationAccountId == accountId, cancellationToken);

        ValidatePaymentRequest(courseIds, enrollments, educationAccount);

        decimal accountCreditBalance = educationAccount!.EducationCreditBalance;
        decimal totalOwed = enrollments.Sum(e => e.Charge!.RemainingAmount);

        decimal balanceToApply = Math.Min(accountCreditBalance, totalOwed);
        decimal remainingToPayViaStripe = totalOwed - balanceToApply;

        var payment = await CreatePayment(accountId, enrollments, educationAccount, totalOwed, balanceToApply, cancellationToken);

        var lineItems = new List<SessionLineItemOptions>();
        var chargeIds = new List<int>();
        var chargeCoveredByCreditBalanceDict = new Dictionary<int, decimal>();
        CreateItemsCheckOut(enrollments, accountCreditBalance, lineItems, chargeIds, chargeCoveredByCreditBalanceDict);

        //Case credit balance is enough to cover the total owed amount, no need to create a Stripe session
        if (remainingToPayViaStripe <= 0)
        {
            await ProcessPaymentInternalAsync(
                payment.Id,
                accountId,
                chargeIds,
                chargeCoveredByCreditBalanceDict,
                remainingToPayViaStripe,
                null,
                targetStatus: PaymentStatus.Succeeded,
                cancellationToken);

            return new PaymentSessionResponseDTO
            {
                Link = "",
                Status = PaymentStatus.Succeeded.ToString()
            };
        }

        var sessionMetadataDto = new StripeSessionMetadataDTO
        {
            AccountId = accountId,
            PaymentId = payment.Id,
            ChargeIds = chargeIds,
            RemainingPaymentViaStripe = remainingToPayViaStripe,
            ChargeCoveredByCreditBalanceDict = chargeCoveredByCreditBalanceDict
        };

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { _stripeSettings.Value.Method },
            LineItems = lineItems,
            Mode = _stripeSettings.Value.Mode,
            SuccessUrl = _stripeSettings.Value.SuccessUrl,
            CancelUrl = _stripeSettings.Value.CancelUrl,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_stripeSettings.Value.SessionExpiryMinutes),
            CustomerEmail = educationAccount.Citizen.Email ?? "",
            Metadata = new Dictionary<string, string>
            {
                { "sessionData", JsonSerializer.Serialize(sessionMetadataDto) }
            }
        };

        var session = await sessionService.CreateAsync(options, cancellationToken: cancellationToken);

        await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
        {
            var trackedPayment = await _paymentRepository
                .Query(tracking: true)
                .FirstAsync(p => p.Id == payment.Id, token);

            trackedPayment.ExternalReference = session.Id;

            await _unitOfWork.SaveChangeAsync(token);
        }, cancellationToken);


        return new PaymentSessionResponseDTO
        {
            Link = session.Url,
            Status = PaymentStatus.Pending.ToString()
        };
    }

    private static void CreateItemsCheckOut(List<Enrollment> enrollments, decimal accountCreditBalance, List<SessionLineItemOptions> lineItems, List<int> chargeIds, Dictionary<int, decimal> chargeCoveredByCreditBalanceDict)
    {
        foreach (var enrollment in enrollments)
        {
            var charge = enrollment.Charge!;
            chargeIds.Add(charge.Id);

            decimal chargeAmountRemaining = charge.RemainingAmount;
            decimal coveredByCreditBalance = Math.Min(chargeAmountRemaining, accountCreditBalance);
            decimal amountToPayViaStripe = chargeAmountRemaining - coveredByCreditBalance;

            accountCreditBalance -= coveredByCreditBalance;

            if (coveredByCreditBalance > 0)
                chargeCoveredByCreditBalanceDict[charge.Id] = coveredByCreditBalance;

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
                            ? $"• Course Fee: {chargeAmountRemaining:C} SGD | • Credit Balance Applied: -{coveredByCreditBalance:C} SGD"
                            : $"• Course Fee: {chargeAmountRemaining:C}"
                    }
                },
                Quantity = 1
            });
        }
    }

    private async Task<Payment> CreatePayment(int accountId, List<Enrollment> enrollments, EducationAccount educationAccount, decimal totalOwed, decimal balanceToApply, CancellationToken cancellationToken)
    {
        Payment payment = null!;
        await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
        {
            payment = new Payment
            {
                Status = PaymentStatus.Pending,
                PaymentMethod = Enums.PaymentMethod.OnlinePayment,
                TotalAmount = totalOwed,
                ExternalReference = null,
                AccountNumberSnapshot = educationAccount.AccountNumber,
                CitizenNricSnapshot = enrollments.First().CitizenNricSnapshot,
                CitizenFullNameSnapshot = enrollments.First().CitizenFullNameSnapshot,
                PaidAt = null
            };
            payment.TryValidate();
            await _paymentRepository.AddAsync(payment, token);
            await _unitOfWork.SaveChangeAsync(token);

            await LogAuditAsync(
            AuditLogCategory.Billing,
           $"Courses Checkout Initiated - PaymentId: {payment.Id}, Credit Balance Applied: {balanceToApply:C}",
           accountId, educationAccount.Citizen.Nric, token);
            await _unitOfWork.SaveChangeAsync(token);

        }, cancellationToken);
        return payment;
    }

    private async Task<PaymentSessionResponseDTO?> ValidateExistingPayment(List<int> courseIds, SessionService sessionService, CancellationToken cancellationToken)
    {
        var existingPayment = await _paymentRepository.Query(tracking: false)
       .Include(p => p.PaymentAllocations)
       .FirstOrDefaultAsync(p =>
        p.Status == PaymentStatus.Pending &&
        p.PaymentMethod == Enums.PaymentMethod.OnlinePayment &&
        p.PaymentAllocations.All(a => courseIds.Contains(a.Charge.Enrollment.CourseId)),
        cancellationToken);

        if (existingPayment != null)
        {
            var existingSession = await sessionService.GetAsync(existingPayment.ExternalReference, cancellationToken: cancellationToken);
            if (existingSession.Status == "open" && existingSession.PaymentStatus == "unpaid")
                return new PaymentSessionResponseDTO
                {
                    Link = existingSession.Url,
                    Status = PaymentStatus.Pending.ToString(),
                };
        }

        return null;
    }

    private static void ValidatePaymentRequest(List<int> requestedCourseIds, List<Enrollment> enrollments, EducationAccount? account)
    {
        var errors = new Dictionary<string, string>();

        if (account == null || account.Status != EducationAccountStatus.Active)
            errors["EducationAccount"] = "User does not have an active education account.";

        var distinctRequestedIds = requestedCourseIds.Distinct().ToList();

        if (enrollments.Count != distinctRequestedIds.Count)
        {
            var foundIds = enrollments.Select(e => e.CourseId).ToList();
            var missingIds = distinctRequestedIds.Except(foundIds);
            errors["CourseIds"] = $"Student is not enrolled in course ID(s): {string.Join(", ", missingIds)}.";
        }

        foreach (var enrollment in enrollments)
        {
            if (enrollment.Course.Status == CourseStatus.Draft)
                errors[$"Course_{enrollment.CourseId}"] = $"Course '{enrollment.CourseNameSnapshot}' is not open for billing.";

            if (enrollment.Charge == null)
            {
                errors[$"Charge_{enrollment.CourseId}"] = $"No billing invoice generated for '{enrollment.CourseNameSnapshot}'.";
                continue;
            }

            if (enrollment.Charge.Status == ChargeStatus.Paid)
                errors[$"Charge_{enrollment.CourseId}"] = $"The charge for '{enrollment.CourseNameSnapshot}' has already paid.";

            if (enrollment.Charge.RemainingAmount <= 0)
                errors[$"Charge_{enrollment.CourseId}"] = $"The remaining amount for '{enrollment.CourseNameSnapshot}' has already been fully paid.";

            //if (enrollment.Charge.PaymentDueDate < DateTime.UtcNow)
            //    errors[$"Charge_{enrollment.CourseId}"] = $"Payment due date for '{enrollment.CourseNameSnapshot}' has passed.";
        }

        if (errors.Count != 0) throw new ValidationFailureException(errors);
    }

    public async Task HandleWebhookAsync(string payload, string stripeSignature)
    {
        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                payload,
                stripeSignature,
                _stripeSettings.Value.WebhookSecret
            );
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
                    await ProcessStripeSessionAsync(completedSession, PaymentStatus.Succeeded, CancellationToken.None);
                break;
            case "checkout.session.expired":
                if (stripeEvent.Data.Object is Session expiredSession)
                    await ProcessStripeSessionAsync(expiredSession, PaymentStatus.Failed, CancellationToken.None);
                break;
        }
    }


    private async Task ProcessStripeSessionAsync(Session session, PaymentStatus targetStatus, CancellationToken cancellationToken)
    {
        var metadata = session.Metadata;
        if (metadata == null || !metadata.ContainsKey("sessionData")) throw new DataNotFoundException($"Session not found for sessionId {session.Id}");

        var sessionData = JsonSerializer.Deserialize<StripeSessionMetadataDTO>(metadata["sessionData"]);
        if (sessionData == null) throw new DataNotFoundException($"Session data not found for sessionId {session.Id}");

        await ProcessPaymentInternalAsync(
           sessionData.PaymentId,
           sessionData.AccountId,
           sessionData.ChargeIds,
           sessionData.ChargeCoveredByCreditBalanceDict,
           sessionData.RemainingPaymentViaStripe,
           session.Id,
           targetStatus,
           cancellationToken);
    }

    private async Task ProcessPaymentInternalAsync(
        int paymentId,
        int accountId,
        List<int> chargeIds,
        Dictionary<int, decimal> chargeCoveredByCreditBalanceDict,
        decimal remainingPaymentViaStripe,
        string? externalReference,
        PaymentStatus targetStatus,
        CancellationToken cancellationToken)
    {
        try
        {
            var charges = await _chargeRepository.Query(tracking: true)
               .Where(c => chargeIds.Contains(c.Id))
               .Include(c => c.Enrollment.Course)
               .OrderBy(c => c.Enrollment.Course.StartDate)
               .ToListAsync(cancellationToken);

            var payment = await _paymentRepository.Query(tracking: true)
                .SingleOrDefaultAsync(p => p.Id == paymentId, cancellationToken);

            var educationAccount = await _accountRepository.Query(tracking: true)
                .Include(a => a.Citizen)
                .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);

            if (educationAccount == null || payment == null || charges.Count == 0)
                throw new InternalAppException("Invalid payment or charge data.");

            // Prevent duplicate processing
            if (payment.Status != PaymentStatus.Pending)
                return;

            bool isSuccess = targetStatus == PaymentStatus.Succeeded;
            decimal totalPaid = 0m;

            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                var paidAt = DateTime.UtcNow.AddHours(8);

                foreach (var charge in charges)
                {
                    decimal chargeAmountRemaining = charge.RemainingAmount;
                    var coveredByCreditBalance = chargeCoveredByCreditBalanceDict.TryGetValue(charge.Id, out var covered)
                        ? covered
                        : 0m;

                    decimal coveredByStripe = chargeAmountRemaining - coveredByCreditBalance;

                    var paymentAllocation = new PaymentAllocation
                    {
                        PaymentId = payment.Id,
                        ChargeId = charge.Id,
                        CourseNameSnapshot = charge.Enrollment.CourseNameSnapshot,
                        SchoolNameSnapshot = charge.Enrollment.SchoolNameSnapshot,
                        ChargeGrossAmountSnapshot = charge.GrossAmount,
                        ChargeNetAmountSnapshot = charge.NetAmount,
                        ChargeRemainingAmountSnapshot = chargeAmountRemaining,
                        Amount = coveredByCreditBalance + coveredByStripe
                    };
                    paymentAllocation.TryValidate();
                    await _paymentAllocationRepository.AddAsync(paymentAllocation, token);

                    totalPaid += chargeAmountRemaining;
                    if (isSuccess)
                    {
                        charge.PaidAmount += chargeAmountRemaining;
                        charge.RemainingAmount = 0;
                        charge.Status = ChargeStatus.Paid;
                    }
                }

                payment.Status = targetStatus;
                payment.ExternalReference = externalReference;
                payment.PaidAt = paidAt;

                var balanceBefore = educationAccount.EducationCreditBalance;
                var totalCreditBalanceCovered = chargeCoveredByCreditBalanceDict.Values.Sum();

                if (totalCreditBalanceCovered > 0 && isSuccess)
                {
                    var balanceAfter = isSuccess ? balanceBefore - totalCreditBalanceCovered : balanceBefore;

                    if (isSuccess)
                        educationAccount.EducationCreditBalance = balanceAfter;

                    var transaction = await CreateTransaction(
                             accountId, isSuccess
                             ? $"Credit balance deduction for paymentId {paymentId}S"
                             : $"Intended credit balance deduction for paymentId {paymentId} was {targetStatus}",
                              totalCreditBalanceCovered, balanceBefore, balanceAfter, token);

                    payment.EducationCreditTransaction = transaction;

                    await LogAuditAsync(
                        AuditLogCategory.Billing,
                        isSuccess
                            ? $"Credit balance deduction for PaymentId: {paymentId}, Credit Balance deducted: {totalCreditBalanceCovered:C}"
                            : $"Credit balance deduction for PaymentId: {paymentId} was {targetStatus}",
                        accountId, educationAccount.Citizen.Nric, cancellationToken);
                }


                await LogAuditAsync(
                AuditLogCategory.Billing,
                $"Processed PaymentId: {paymentId} was completed with status {targetStatus}",
                accountId, educationAccount.Citizen.Nric, cancellationToken);

                await HandelSendEmail(
                    targetStatus, payment, educationAccount, isSuccess,
                    totalPaid, totalCreditBalanceCovered, remainingPaymentViaStripe);

                await _unitOfWork.SaveChangeAsync(token);
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            await LogAuditAsync(
                AuditLogCategory.Billing,
                $"Error processing paymentId: {paymentId}",
                accountId, null, cancellationToken);
            await _unitOfWork.SaveChangeAsync();

            throw new InternalAppException("An error occurred while processing the payment.", ex.InnerException);
        }
    }

    private async Task HandelSendEmail(PaymentStatus targetStatus, Payment payment, EducationAccount educationAccount, bool isSuccess, decimal totalPaid, decimal totalWalletCovered, decimal totalStripeCovered)
    {
        var subject = isSuccess ? "Payment Confirmed - MOS" : $"Payment {targetStatus} - MOS";
        var subMessage = isSuccess
        ? $"Deduction from your credit balance: {totalWalletCovered:C} SGD and deduction via online payment: {totalStripeCovered:C} SGD"
        : "Contact MOS Staff for more information!";
        var message = isSuccess
            ? $"Dear {payment.CitizenFullNameSnapshot}, your payment of {totalPaid:C} SGD has been confirmed." + subMessage
            : $"Dear {payment.CitizenFullNameSnapshot}, your payment session has been {targetStatus}." + subMessage;

        await _outboxWriter.EnqueueEmailAsync(
            educationAccount.Citizen.Email!,
            new EmailTemplate(Subject: subject, HtmlBody: message, TextBody: message)
        );

        //await _emailService.SendAsync(
        //    educationAccount.Citizen.Email!,
        //    new EmailTemplate(Subject: subject, HtmlBody: message, TextBody: message),
        //    cancellationToken
        //);
    }

    private async Task<EducationCreditTransaction> CreateTransaction(
        int accountId, string description,
        decimal amount, decimal balanceBefore,
        decimal balanceAfter, CancellationToken token)
    {
        var transaction = new EducationCreditTransaction
        {
            Type = EducationCreditTransactionType.CourseFeePayment,
            Direction = EducationCreditTransactionDirection.Debit,
            Amount = amount,
            BalanceBefore = balanceBefore,
            BalanceAfter = balanceAfter,
            Description = description,
            EducationAccountId = accountId
        };
        transaction.TryValidate();
        await _transactionRepository.AddAsync(transaction, token);
        return transaction;
    }

    public async Task<PaymentSessionResponseDTO> HandleSessionSuccessAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.Query(tracking: false)
            .FirstOrDefaultAsync(p => p.ExternalReference == sessionId, cancellationToken);
        if (payment == null) throw new DataNotFoundException($"Session not found for sessionId {sessionId}");

        return new PaymentSessionResponseDTO
        {
            Link = "",
            Status = payment.Status.ToString()
        };
    }

    public async Task<PaymentSessionResponseDTO> HandleSessionCancelledAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var sessionService = new SessionService();
        var session = await sessionService.GetAsync(sessionId, cancellationToken: cancellationToken);
        if (session == null) throw new DataNotFoundException($"Session not found for sessionId {sessionId}");

        await ProcessStripeSessionAsync(session, PaymentStatus.Canceled, cancellationToken);

        return new PaymentSessionResponseDTO
        {
            Link = "",
            Status = PaymentStatus.Canceled.ToString()
        };

    }


    private async Task LogAuditAsync(AuditLogCategory category, string action, int accountId, string? nric, CancellationToken cancellationToken)
    {
        await _auditLogWriter.LogAsync(category, action, nric, cancellationToken);
    }
}
