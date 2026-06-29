namespace Interfaces.Audit
{
    public interface IManagementActionLogService
    {
        Task LogAsync(
            Guid batchId,
            string entityType,
            int entityId,
            string action,
            string reason,
            string? previousStatus = null,
            string? newStatus = null,
            string? metadataJson = null,
            CancellationToken cancellationToken = default);
    }
}
