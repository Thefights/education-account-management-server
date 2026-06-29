using DTOs.Audit;
using Interfaces.Base;

namespace Interfaces.Audit
{
    public interface IManagementActionLogService : IBaseGetService<GetManagementActionLogDTO>
    {
        Task LogAsync(
            Guid batchId,
            ManagementActionEntityType entityType,
            int entityId,
            ManagementAction action,
            string reason,
            string? previousStatus = null,
            string? newStatus = null,
            CancellationToken cancellationToken = default);

        Task<byte[]> ExportCsvAsync(
            ExportManagementActionLogRequestDTO request,
            CancellationToken cancellationToken = default);
    }
}
