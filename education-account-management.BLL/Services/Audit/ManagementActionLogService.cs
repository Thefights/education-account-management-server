using DTOs.Audit;
using Interfaces.Audit;
using Interfaces.Csv;
using Services.Base;

namespace Services.Audit
{
    public class ManagementActionLogService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        ManagementActionLogMapper mapper,
        ICsvExportService csvExportService)
        : BaseGetService<ManagementActionLog, GetManagementActionLogDTO>(
            unitOfWork,
            mapper,
            [
                nameof(ManagementActionLog.ActorUser),
                $"{nameof(ManagementActionLog.ActorUser)}.{nameof(User.AdminProfile)}"
            ]),
            IManagementActionLogService
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ManagementActionLogMapper _mapper = mapper;
        private readonly ICsvExportService _csvExportService = csvExportService;

        private static readonly IReadOnlyDictionary<string, CsvExportColumn<GetManagementActionLogDTO>> ExportColumns =
            new Dictionary<string, CsvExportColumn<GetManagementActionLogDTO>>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = new("Id", log => log.Id),
                ["batchId"] = new("Batch Id", log => log.BatchId),
                ["entityType"] = new("Entity Type", log => log.EntityType),
                ["entityId"] = new("Entity Id", log => log.EntityId),
                ["action"] = new("Action", log => log.Action),
                ["previousStatus"] = new("Previous Status", log => log.PreviousStatus),
                ["newStatus"] = new("New Status", log => log.NewStatus),
                ["reason"] = new("Reason", log => log.Reason),
                ["actorUserId"] = new("Actor User Id", log => log.ActorUserId),
                ["actorUserRole"] = new("Actor User Role", log => log.ActorUserRole),
                ["actorFullName"] = new("Actor Full Name", log => log.ActorFullName),
                ["actorEmail"] = new("Actor Email", log => log.ActorEmail),
                ["occurredAt"] = new("Occurred At", log => log.OccurredAt),
                ["ipAddress"] = new("IP Address", log => log.IpAddress)
            };

        public async Task LogAsync(
            Guid batchId,
            ManagementActionEntityType entityType,
            int entityId,
            ManagementAction action,
            string reason,
            string? previousStatus = null,
            string? newStatus = null,
            CancellationToken cancellationToken = default)
        {
            var log = new ManagementActionLog
            {
                BatchId = batchId,
                EntityType = entityType,
                EntityId = entityId,
                Action = action,
                PreviousStatus = previousStatus,
                NewStatus = newStatus,
                Reason = reason,
                ActorUserId = _currentUserService.CurrentUserId,
                OccurredAt = DateTime.UtcNow,
                IpAddress = _currentUserService.IpAddress
            };

            log.TryValidate();
            await _repository.AddAsync(log, cancellationToken);
        }

        public async Task<byte[]> ExportCsvAsync(
            ExportManagementActionLogRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Filter);

            var records = await _repository.GetProjectedFilteredAsync(
                _mapper.ProjectToGetDTO,
                request.Filter.Filter,
                request.Filter.Search,
                request.Filter.SearchFields,
                request.Filter.SortExpression,
                _includes,
                cancellationToken: cancellationToken);

            return _csvExportService.Export(records, request.Fields, ExportColumns);
        }
    }
}
