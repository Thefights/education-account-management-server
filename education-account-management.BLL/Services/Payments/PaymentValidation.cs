using DTOs.Payments;
using Stripe.Checkout;
using System.Text.Json;
using Charge = Models.Charge;
using PaymentIntent = Enums.PaymentIntent;

namespace Services.Payments;

public partial class StripeService
{
    // ===== Cụm 5: Checkout validation =====
    // Validation here is business/cross-aggregate validation. Simple DTO/entity validation
    // remains handled by existing annotations and TryValidate calls.
    private async Task<PendingSessionLookup> FindMatchingActivePendingSessionAsync(
        List<ChargeBillingActionItem> requestedActions,
        CancellationToken cancellationToken)
    {
        var requestedChargeIds = requestedActions.Select(action => action.ChargeId).ToList();
        var pendingPayments = await _paymentRepository.Query(tracking: true)
            .Include(payment => payment.PaymentAllocations)
            .Where(payment => payment.Status == PaymentStatus.Pending
                && payment.PaymentAllocations.Any(allocation => requestedChargeIds.Contains(allocation.ChargeId)))
            .ToListAsync(cancellationToken);

        if (pendingPayments.Count == 0) return new PendingSessionLookup(null, null);

        var paidSession = await CleanupExpiredPendingPaymentsAsync(pendingPayments, cancellationToken);
        if (paidSession != null) return new PendingSessionLookup(null, paidSession);

        var blockingPayments = pendingPayments
            .Where(payment => payment.Status == PaymentStatus.Pending)
            .ToList();
        foreach (var sessionGroup in blockingPayments.GroupBy(payment => NormalizeSessionReference(payment.ExternalReference)))
        {
            var groupedPayments = sessionGroup.ToList();
            var sessionReference = sessionGroup.Key;
            if (string.IsNullOrWhiteSpace(sessionReference) ||
                sessionReference.StartsWith("RESERVING_", StringComparison.Ordinal))
            {
                continue;
            }

            var session = await _stripeCheckoutGateway.GetAsync(sessionReference, cancellationToken);
            if (session?.PaymentStatus == "paid")
            {
                return new PendingSessionLookup(null, session);
            }

            if (session?.Status != "open" || string.IsNullOrWhiteSpace(session.Url))
            {
                foreach (var payment in sessionGroup)
                    payment.Status = PaymentStatus.Failed;

                continue;
            }

            var allocatedChargeIds = groupedPayments
                .SelectMany(payment => payment.PaymentAllocations)
                .Select(allocation => allocation.ChargeId)
                .Distinct()
                .OrderBy(chargeId => chargeId)
                .ToList();
            var expectedChargeIds = requestedChargeIds
                .Distinct()
                .OrderBy(chargeId => chargeId)
                .ToList();

            if (allocatedChargeIds.SequenceEqual(expectedChargeIds) &&
                SessionMatchesRequestedActions(session, requestedActions))
            {
                return new PendingSessionLookup(
                    PaymentSessionResponseFactory.FromPayments(
                        groupedPayments, PaymentStatus.Pending, session.Url),
                    null);
            }
        }

        blockingPayments = blockingPayments
            .Where(payment => payment.Status == PaymentStatus.Pending)
            .ToList();
        if (blockingPayments.Count > 0)
        {
            var retryAt = blockingPayments.Max(GetPendingPaymentExpiryAt);
            var remainingMinutes = Math.Max(1, (int)Math.Ceiling((retryAt - DateTime.UtcNow).TotalMinutes));
            throw new ValidationFailureException(new Dictionary<string, string>
            {
                {
                    "ChargeIds",
                    $"One or more selected {nameof(Course)}s are locked by a pending {nameof(Payment)}. " +
                    $"Retry after {retryAt:yyyy-MM-dd HH:mm:ss} UTC (about {remainingMinutes} minute(s)), " +
                    "or complete/cancel the existing checkout session."
                }
            });
        }

        return new PendingSessionLookup(null, null);
    }

    private static bool SessionMatchesRequestedActions(
        Session session,
        List<ChargeBillingActionItem> requestedActions)
    {
        if (!session.Metadata.TryGetValue("sessionData", out var sessionData) ||
            string.IsNullOrWhiteSpace(sessionData))
        {
            return false;
        }

        StripeSessionMetadataDTO? metadata;
        try
        {
            metadata = JsonSerializer.Deserialize<StripeSessionMetadataDTO>(sessionData);
        }
        catch (JsonException)
        {
            return false;
        }

        if (metadata?.BillingActions.Count != requestedActions.Count)
            return false;

        var existingActions = metadata.BillingActions
            .OrderBy(action => action.ChargeId)
            .Select(action => (action.ChargeId, action.Intent, action.PaymentPlanMonths, action.InstallmentCount))
            .ToList();
        var expectedActions = requestedActions
            .OrderBy(action => action.ChargeId)
            .Select(action => (action.ChargeId, action.Intent, action.PaymentPlanMonths, action.InstallmentCount))
            .ToList();

        return existingActions.SequenceEqual(expectedActions);
    }

    private async Task<Session?> CleanupExpiredPendingPaymentsAsync(
        List<Payment> pendingPayments,
        CancellationToken cancellationToken)
    {
        var utcNow = DateTime.UtcNow;
        var expiredPayments = pendingPayments
            .Where(payment => IsPendingPaymentExpired(payment, utcNow))
            .ToList();

        if (expiredPayments.Count == 0) return null;

        foreach (var sessionGroup in expiredPayments.GroupBy(payment => NormalizeSessionReference(payment.ExternalReference)))
        {
            var sessionReference = sessionGroup.Key;
            if (!string.IsNullOrWhiteSpace(sessionReference) && !sessionReference.StartsWith("RESERVING_", StringComparison.Ordinal))
            {
                var session = await _stripeCheckoutGateway.GetAsync(sessionReference, cancellationToken);
                if (session?.PaymentStatus == "paid")
                {
                    return session;
                }

                if (session?.Status == "open")
                {
                    await _stripeCheckoutGateway.ExpireAsync(sessionReference, cancellationToken);
                }
            }

            foreach (var payment in sessionGroup)
            {
                payment.Status = PaymentStatus.Failed;
            }
        }

        return null;
    }

    private bool IsPendingPaymentExpired(Payment payment, DateTime utcNow)
    {
        return GetPendingPaymentExpiryAt(payment) <= utcNow;
    }

    private DateTime GetPendingPaymentExpiryAt(Payment payment)
    {
        var expiryMinutes = payment.ExternalReference?.StartsWith("RESERVING_", StringComparison.Ordinal) == true
            ? 2
            : _configuration.StripeConfig.SessionExpiryMinutes;

        return payment.CreatedAt.AddMinutes(expiryMinutes);
    }

    private static string? NormalizeSessionReference(string? externalReference)
    {
        if (string.IsNullOrWhiteSpace(externalReference)) return null;
        return externalReference.EndsWith("_wallet", StringComparison.Ordinal)
            ? externalReference[..^"_wallet".Length]
            : externalReference;
    }

    /// <summary>
    /// Kiểm tra tính hợp lệ của Request theo các luật Business (Ví dụ: Không tạo 2 luồng trả góp, trả đúng kỳ, v.v.)
    /// </summary>
    private static void ValidateBillingActions(List<ChargeBillingActionItem> billingActions, List<Charge> charges, EducationAccount educationAccount, decimal creditBalanceApplied, DateTime utcNow)
    {
        var errors = new Dictionary<string, string>();

        if (educationAccount.Status != EducationAccountStatus.Active)
            errors[$"{nameof(EducationAccount)}"] = $"User does not have an active {nameof(EducationAccount)}";
        if (creditBalanceApplied > educationAccount.EducationCreditBalance)
            errors["CreditBalanceApplied"] = "Applied balance exceeds available credit balance.";

        var chargeIds = billingActions.Select(r => r.ChargeId).ToList();
        var distinctRequestedIds = chargeIds.Distinct().ToList();
        if (distinctRequestedIds.Count != chargeIds.Count)
        {
            errors[$"{nameof(Charge)}Ids"] = $"Duplicate {nameof(Charge)} IDs are not allowed in a payment request.";
        }

        if (charges.Count != distinctRequestedIds.Count)
        {
            var foundIds = charges.Select(c => c.Id).ToList();
            var missingIds = distinctRequestedIds.Except(foundIds);
            errors[$"{nameof(Charge)}Ids"] = $"{nameof(Charge)} ID(s) not found or not belonging to user: {string.Join(", ", missingIds)}.";
        }

        foreach (var charge in charges)
        {
            var enrollment = charge.Enrollment;
            var info = billingActions.First(ci => ci.ChargeId == charge.Id);

            if (enrollment.Course.Status == CourseStatus.Draft)
                errors[$"{nameof(Course)}_{enrollment.CourseId}"] = $"{nameof(Course)} '{enrollment.CourseNameSnapshot}' is not open for billing.";
            if (charge.Status == ChargeStatus.Paid || charge.RemainingAmount <= 0)
                errors[$"{nameof(Charge)}_{charge.Id}"] = $"The {nameof(Charge)} for '{enrollment.CourseNameSnapshot}' has already been fully paid.";

            bool hasUnpaidInstallments = charge.Installments.Any();
            bool hasUnlockedInstallment = charge.Installments.Any(installment =>
                IsInstallmentDueForPayment(installment, utcNow));
            bool isChargeDueForPayment = PaymentDueWindow.IsDueForPayment(
                charge.CourseEndDateSnapshot,
                utcNow);

            switch (info.Intent)
            {
                case PaymentIntent.PayFull:
                    if (!isChargeDueForPayment)
                    {
                        errors[$"{nameof(Charge)}_{charge.Id}_NotDue"] =
                            $"The {nameof(Charge)} for '{enrollment.CourseNameSnapshot}' is not due for payment yet.";
                        break;
                    }
                    if (hasUnpaidInstallments || charge.PaymentPlanMonths.HasValue)
                        errors[$"{nameof(Charge)}_{charge.Id}"] = $"Cannot {nameof(PaymentIntent.PayFull)} while an installment plan is active. Use {nameof(PaymentIntent.PayDueInstallments)} instead.";
                    break;
                case PaymentIntent.CreateInstallment:
                    if (!isChargeDueForPayment)
                    {
                        errors[$"{nameof(Charge)}_{charge.Id}_NotDue"] =
                            $"The {nameof(Charge)} for '{enrollment.CourseNameSnapshot}' is not due for payment yet.";
                        break;
                    }
                    if (hasUnpaidInstallments || charge.PaymentPlanMonths.HasValue)
                    {
                        errors[$"{nameof(Charge)}_{charge.Id}_PlanExists"] = $"Cannot create new {nameof(ChargeInstallment)} plan. An {nameof(ChargeInstallment)} plan already exists.";
                        break;
                    }
                    break;
                case PaymentIntent.PayDueInstallments:
                    if (!hasUnpaidInstallments || !charge.PaymentPlanMonths.HasValue)
                    {
                        errors[$"{nameof(Charge)}_{charge.Id}_MissingPlan"] = $"Cannot pay due {nameof(ChargeInstallment)}s because no active {nameof(ChargeInstallment)} plan exists.";
                        break;
                    }
                    if (!hasUnlockedInstallment)
                    {
                        errors[$"{nameof(Charge)}_{charge.Id}_NoInstallmentDue"] = $"No {nameof(ChargeInstallment)} is due yet for '{enrollment.CourseNameSnapshot}'. Future installments can only be paid through {nameof(PaymentIntent.PayRemainingInstallments)}.";
                        break;
                    }
                    var unlockedInstallmentCount = charge.Installments.Count(installment =>
                        IsInstallmentDueForPayment(installment, utcNow));
                    if (info.InstallmentCount is null or < 1 || info.InstallmentCount > unlockedInstallmentCount)
                    {
                        errors[$"{nameof(Charge)}_{charge.Id}_InstallmentCount"] =
                            $"Installment count must be between 1 and {unlockedInstallmentCount} for '{enrollment.CourseNameSnapshot}'.";
                    }
                    break;
                case PaymentIntent.PayRemainingInstallments:
                    if (!hasUnpaidInstallments || !charge.PaymentPlanMonths.HasValue)
                    {
                        errors[$"{nameof(Charge)}_{charge.Id}_MissingPlan"] = $"No active {nameof(ChargeInstallment)} plan exists to pay off remaining installments. Or remaining {nameof(ChargeInstallment)} already paid";
                        break;
                    }
                    break;
            }
        }

        if (errors.Count != 0) throw new ValidationFailureException(errors);
    }

    private static bool IsInstallmentDueForPayment(ChargeInstallment installment, DateTime utcNow)
    {
        return installment.Status != ChargeInstallmentStatus.Paid &&
               PaymentDueWindow.IsDueForPayment(installment.DueDate, utcNow);
    }
}
