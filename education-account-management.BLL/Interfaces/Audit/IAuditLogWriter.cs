using Enums;

namespace Interfaces.Audit
{
    public interface IAuditLogWriter
    {
        Task LogAsync(
            AuditLogCategory category,
            string action,
            string? targetNric = null,
            CancellationToken cancellationToken = default);

        Task LogAnonymousAsync(
            AuditLogCategory category,
            string action,
            string? targetNric = null,
            string? ipAddress = null,
            CancellationToken cancellationToken = default);
    }
}
