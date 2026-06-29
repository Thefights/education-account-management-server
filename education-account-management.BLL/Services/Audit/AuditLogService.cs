using DTOs.Audit;
using Interfaces.Audit;
using Interfaces.Csv;
using Services.Base;

namespace Services.Audit
{
    public class AuditLogService(
        IUnitOfWork unitOfWork,
        AuditLogMapper auditLogMapper,
        ICsvExportService csvExportService,
        IAuditLogWriter auditLogWriter)
        : BaseGetService<AuditLog, GetAuditLogDTO>(unitOfWork, auditLogMapper, includes: [nameof(AuditLog.ActorUser)]),
            IAuditLogService
    {
        private readonly AuditLogMapper _auditLogMapper = auditLogMapper;
        private readonly ICsvExportService _csvExportService = csvExportService;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;

        private static readonly IReadOnlyDictionary<string, CsvExportColumn<GetAuditLogDTO>> ExportColumns =
            new Dictionary<string, CsvExportColumn<GetAuditLogDTO>>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = new("Id", auditLog => auditLog.Id),
                ["actorUserId"] = new("Actor User Id", auditLog => auditLog.ActorUserId),
                ["actorUserRole"] = new("Actor User Role", auditLog => auditLog.ActorUserRole),
                ["category"] = new("Category", auditLog => auditLog.Category),
                ["action"] = new("Action", auditLog => auditLog.Action),
                ["ipAddress"] = new("IP Address", auditLog => auditLog.IpAddress),
                ["occurredAt"] = new("Occurred At", auditLog => auditLog.OccurredAt),
                ["nric"] = new("NRIC", auditLog => auditLog.Nric)
            };

        public async Task<byte[]> ExportCsvAsync(
            ExportAuditLogRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Filter);

            var records = await _repository.GetProjectedFilteredAsync(
                _auditLogMapper.ProjectToGetDTO,
                request.Filter.Filter,
                request.Filter.Search,
                request.Filter.SearchFields,
                request.Filter.SortExpression,
                cancellationToken: cancellationToken);

            var content = _csvExportService.Export(records, request.Fields, ExportColumns);
            await _auditLogWriter.LogAsync(
                AuditLogCategory.Security,
                "ViewAuditLogs",
                cancellationToken: cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return content;
        }

    }
}
