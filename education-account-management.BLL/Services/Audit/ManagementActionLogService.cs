using Interfaces.Audit;

namespace Services.Audit
{
    public class ManagementActionLogService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService) : IManagementActionLogService
    {
        private readonly IGenericRepository<ManagementActionLog> _repository =
            unitOfWork.Repository<ManagementActionLog>();
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task LogAsync(
            Guid batchId,
            string entityType,
            int entityId,
            string action,
            string reason,
            string? previousStatus = null,
            string? newStatus = null,
            string? metadataJson = null,
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
                IpAddress = _currentUserService.IpAddress,
                MetadataJson = metadataJson
            };

            log.TryValidate();
            await _repository.AddAsync(log, cancellationToken);
        }
    }
}
