using DTOs.Payments;
using Stripe;
using Stripe.Checkout;
using System.Text.Json;

namespace Services.Payments;

public partial class StripeService
{
    // ===== Cụm 6: Stripe callback/session entrypoints =====
    // Webhook, success URL and cancel URL all converge into ProcessStripeSessionAsync,
    // then finalization applies only pending payments to keep callbacks idempotent.
    /// <summary>
    /// Webhook đích: Lắng nghe sự kiện từ Stripe trả về để xử lý cập nhật trạng thái đơn hàng.
    /// </summary>
    public async Task HandleWebhookAsync(string payload, string stripeSignature)
    {
        Event stripeEvent;
        try
        {
            stripeEvent = _stripeCheckoutGateway.ConstructEvent(
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
                if (stripeEvent.Data.Object is Session completedSession &&
                    completedSession.PaymentStatus == "paid")
                    await ProcessStripeSessionAsync(null, completedSession, PaymentStatus.Succeeded, CancellationToken.None);
                break;
            case "checkout.session.expired":
                if (stripeEvent.Data.Object is Session expiredSession)
                    await ProcessStripeSessionAsync(null, expiredSession, PaymentStatus.Failed, CancellationToken.None);
                break;
        }
    }

    private async Task ProcessStripeSessionAsync(int? accountId, Session session, PaymentStatus targetStatus, CancellationToken cancellationToken)
    {
        var sessionData = GetSessionMetadata(session);
        if (sessionData == null) throw new DataNotFoundException($"Session data not found for sessionId {session.Id}");
        if (accountId != null && sessionData.AccountId != accountId) throw new DataConflictException($"Current User not belong to payment Session {session.Id}");

        var billingActions = sessionData.BillingActions
            .Select(action => new ChargeBillingActionItem(action.ChargeId, action.Intent, action.PaymentPlanMonths))
            .ToList();
        var paymentIds = await GetPaymentIdsBySessionReferenceAsync(session.Id, cancellationToken);

        await ProcessPaymentInternalAsync(paymentIds, sessionData.AccountId, session.Id, billingActions, targetStatus, cancellationToken);
    }

    private static StripeSessionMetadataDTO? GetSessionMetadata(Session session)
    {
        var metadata = session.Metadata;
        if (metadata == null || !metadata.TryGetValue("sessionData", out var sessionDataJson))
            throw new DataNotFoundException($"Session not found for sessionId {session.Id}");

        return JsonSerializer.Deserialize<StripeSessionMetadataDTO>(sessionDataJson);
    }

    private async Task<List<int>> GetPaymentIdsBySessionReferenceAsync(string sessionId, CancellationToken cancellationToken)
    {
        var paymentIds = await _paymentRepository.Query(tracking: false)
            .Where(payment => payment.ExternalReference == sessionId || payment.ExternalReference == sessionId + "_wallet")
            .Select(payment => payment.Id)
            .ToListAsync(cancellationToken);

        if (paymentIds.Count == 0)
            throw new DataNotFoundException($"No {nameof(Payment)} found for sessionId {sessionId}");

        return paymentIds;
    }

    // ===== Cụm 10: Notification and direct browser callbacks =====
    // Email/audit are side effects after finalization. Success/cancel browser callbacks re-read
    // Stripe session state and delegate to the same internal finalization path as webhook.
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
            educationAccount.Citizen.Email ?? "",
            new EmailTemplate(Subject: subject, HtmlBody: message, TextBody: message)
        );
    }

    public async Task<PaymentSessionResponseDTO> HandleSessionSuccessAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var (session, educationAccount) = await GetSessionAndAccountAsync(sessionId, cancellationToken);

        var payments = await _paymentRepository
            .Query(tracking: false)
            .Where(p =>
             (p.ExternalReference == sessionId || p.ExternalReference == sessionId + "_wallet") &&
              p.AccountNumberSnapshot == educationAccount.AccountNumber)
            .ToListAsync(cancellationToken);

        if (payments == null || !payments.Any()) throw new DataNotFoundException($"Session not found or invalid {nameof(Payment)}s count for sessionId {sessionId}");

        if (payments.Any(payment => payment.Status == PaymentStatus.Pending))
        {
            if (session.PaymentStatus == "paid")
            {
                await ProcessStripeSessionAsync(educationAccount.Id, session, PaymentStatus.Succeeded, cancellationToken);
                return await BuildSessionResponseAsync(sessionId, PaymentStatus.Succeeded, cancellationToken);
            }
        }

        return PaymentSessionResponseFactory.FromPayments(payments, payments[0].Status);
    }

    public async Task<PaymentSessionResponseDTO> HandleSessionCancelledAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var (session, educationAccount) = await GetSessionAndAccountAsync(sessionId, cancellationToken);

        if (session.PaymentStatus == "paid")
        {
            await ProcessStripeSessionAsync(educationAccount.Id, session, PaymentStatus.Succeeded, cancellationToken);
            return await BuildSessionResponseAsync(sessionId, PaymentStatus.Succeeded, cancellationToken);
        }

        if (session.Status == "open")
        {
            session = await _stripeCheckoutGateway.ExpireAsync(sessionId, cancellationToken);
        }

        if (session.PaymentStatus == "paid")
        {
            await ProcessStripeSessionAsync(educationAccount.Id, session, PaymentStatus.Succeeded, cancellationToken);
            return await BuildSessionResponseAsync(sessionId, PaymentStatus.Succeeded, cancellationToken);
        }

        if (session.Status == "expired" && session.PaymentStatus == "unpaid")
        {
            await ProcessStripeSessionAsync(educationAccount.Id, session, PaymentStatus.Canceled, cancellationToken);
            return await BuildSessionResponseAsync(sessionId, PaymentStatus.Canceled, cancellationToken);
        }

        throw new DataConflictException($"Stripe session {sessionId} cannot be canceled in status {session.Status}/{session.PaymentStatus}.");
    }

    private async Task<PaymentSessionResponseDTO> BuildSessionResponseAsync(
        string sessionId,
        PaymentStatus fallbackStatus,
        CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.Query(tracking: false)
            .Where(payment => payment.ExternalReference == sessionId || payment.ExternalReference == sessionId + "_wallet")
            .ToListAsync(cancellationToken);

        if (payments.Count == 0)
            throw new DataNotFoundException($"No {nameof(Payment)} found for sessionId {sessionId}");

        var distinctStatuses = payments.Select(payment => payment.Status).Distinct().ToList();
        var status = distinctStatuses.Count == 1 ? distinctStatuses[0] : fallbackStatus;

        return PaymentSessionResponseFactory.FromPayments(payments, status);
    }

    private async Task<(Session session, EducationAccount educationAccount)> GetSessionAndAccountAsync(string sessionId, CancellationToken cancellationToken)
    {
        var session = await _stripeCheckoutGateway.GetAsync(sessionId, cancellationToken);
        if (session == null) throw new DataNotFoundException($"Session not found for sessionId {sessionId}");

        var currentUserId = _currentUserService.UserId;
        var educationAccount = await _accountRepository.Query(tracking: false)
            .FirstOrDefaultAsync(a => a.Citizen != null && a.Citizen.User != null && a.Citizen.User.Id == currentUserId, cancellationToken);

        if (educationAccount == null) throw new InternalAppException("Account holder not found in process session!");

        return (session, educationAccount);
    }

    /// <summary>
    /// Ghi nhận lịch sử (Audit Log) cho các hành động quan trọng (giới hạn Action dưới 100 ký tự).
    /// </summary>
    private async Task LogAuditAsync(AuditLogCategory category, string action, int accountId, string? nric, CancellationToken cancellationToken)
    {
        await _auditLogWriter.LogAsync(category, action, nric, cancellationToken);
    }
}
