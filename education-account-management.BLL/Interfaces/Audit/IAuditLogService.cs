using DTOs.Audit;
using Interfaces.Base;

namespace Interfaces.Audit
{
    public interface IAuditLogService : IBaseGetService<GetAuditLogDTO>
    {
        Task<byte[]> ExportCsvAsync(
            ExportAuditLogRequestDTO request,
            CancellationToken cancellationToken = default);
    }

}
