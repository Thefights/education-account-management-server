using Emails;
using Enums;
using Infrastructure.Interface;
using Interfaces.Audit;
using Interfaces.Email;
using Interfaces.Notifications;
using Interfaces.Payments;
using Repositories.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace education_account_management.Tests.Fakes;

internal sealed class TestAuditUserContext(int? currentUserId = null) : IAuditUserContext
{
    public int? CurrentUserId { get; } = currentUserId;
}

internal sealed class TestCurrentUserService(int userId) : ICurrentUserService
{
    public int UserId { get; } = userId;
    public int? CurrentUserId => UserId;
    public UserRole Role => UserRole.AccountHolder;
    public string UserName => "test-account-holder";
    public string IpAddress => "127.0.0.1";
}

internal sealed class NoopAuditLogWriter : IAuditLogWriter
{
    public List<(AuditLogCategory Category, string Action, string? TargetNric)> Entries { get; } = [];

    public Task LogAsync(
        AuditLogCategory category,
        string action,
        string? targetNric = null,
        CancellationToken cancellationToken = default)
    {
        Entries.Add((category, action, targetNric));
        return Task.CompletedTask;
    }

    public Task LogAnonymousAsync(
        AuditLogCategory category,
        string action,
        string? targetNric = null,
        string? ipAddress = null,
        CancellationToken cancellationToken = default)
    {
        Entries.Add((category, action, targetNric));
        return Task.CompletedTask;
    }
}

internal sealed class NoopOutboxWriter : IOutboxWriter
{
    public List<(string ToEmail, EmailTemplate Template)> Emails { get; } = [];

    public Task EnqueueEmailAsync(
        string toEmail,
        EmailTemplate template,
        CancellationToken cancellationToken = default)
    {
        Emails.Add((toEmail, template));
        return Task.CompletedTask;
    }

    public Task EnqueueEmailOnceAsync(
        string toEmail,
        EmailTemplate template,
        CancellationToken cancellationToken = default)
    {
        if (!Emails.Any(email =>
                email.ToEmail == toEmail
                && email.Template == template))
        {
            Emails.Add((toEmail, template));
        }

        return Task.CompletedTask;
    }
}

internal sealed class NoopNotificationWriter : INotificationWriter
{
    public Task CreateAsync(
        int recipientUserId,
        NotificationType type,
        NotificationSeverity severity,
        string title,
        string message,
        string? relatedEntityType = null,
        int? relatedEntityId = null,
        object? metadata = null,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task CreateForUsersAsync(
        IReadOnlyCollection<int> recipientUserIds,
        NotificationType type,
        NotificationSeverity severity,
        string title,
        string message,
        string? relatedEntityType = null,
        int? relatedEntityId = null,
        object? metadata = null,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}

internal sealed class FakeStripeCheckoutGateway : IStripeCheckoutGateway
{
    private readonly Dictionary<string, Session> _sessions = [];
    private int _nextSessionNumber = 1;

    public int CreateCallCount { get; private set; }
    public int ExpireCallCount { get; private set; }
    public SessionCreateOptions? LastCreateOptions { get; private set; }
    public Exception? CreateException { get; set; }
    public Exception? GetException { get; set; }
    public bool CompletePaymentOnExpire { get; set; }

    public Task<Session> CreateAsync(
        SessionCreateOptions options,
        CancellationToken cancellationToken = default)
    {
        if (CreateException != null) throw CreateException;

        CreateCallCount++;
        LastCreateOptions = options;
        var session = new Session
        {
            Id = $"cs_test_{_nextSessionNumber++}",
            Url = "https://stripe.test/checkout",
            Status = "open",
            PaymentStatus = "unpaid",
            Metadata = options.Metadata
        };
        _sessions[session.Id] = session;
        return Task.FromResult(session);
    }

    public Task<Session> GetAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        if (GetException != null) throw GetException;

        return Task.FromResult(_sessions[sessionId]);
    }

    public Task<Session> ExpireAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        ExpireCallCount++;
        var session = _sessions[sessionId];
        if (CompletePaymentOnExpire)
        {
            session.PaymentStatus = "paid";
            session.Status = "complete";
        }
        else
        {
            session.Status = "expired";
        }
        return Task.FromResult(session);
    }

    public Event ConstructEvent(string payload, string signature, string webhookSecret)
    {
        throw new NotSupportedException("Webhook signature validation is not used in these tests.");
    }

    public void MarkPaid(string sessionId)
    {
        var session = _sessions[sessionId];
        session.PaymentStatus = "paid";
        session.Status = "complete";
    }

    public void KeepUnpaidOpen(string sessionId)
    {
        var session = _sessions[sessionId];
        session.PaymentStatus = "unpaid";
        session.Status = "open";
    }
}
