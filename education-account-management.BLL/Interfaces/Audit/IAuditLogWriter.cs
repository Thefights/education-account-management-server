namespace Interfaces.Audit
{
    public interface IAuditLogWriter
    {
        Task LogAsync(
            AuditLogCategory category,
            AuditLogAction action,
            string? objectText,
            CancellationToken cancellationToken = default);

        Task LogForActorAsync(
            AuthAccount actor,
            AuditLogCategory category,
            AuditLogAction action,
            string? objectText,
            string? ipAddress = null,
            CancellationToken cancellationToken = default);

        Task LogAnonymousAsync(
            AuditLogCategory category,
            AuditLogAction action,
            string? objectText,
            string actorUserIdText = "Anonymous",
            string actorFullName = "Anonymous",
            string? ipAddress = null,
            CancellationToken cancellationToken = default);
    }
}
