using Enums;
using Infrastructure.Interface;
using Interfaces.Audit;
using Models;
using Repositories.Interfaces;

namespace Services.Audit
{
    public class AuditLogWriter(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IHttpContextAccessor httpContextAccessor)
        : IAuditLogWriter
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IGenericRepository<AuditLog> _auditLogRepository = unitOfWork.Repository<AuditLog>();
        private readonly IGenericRepository<User> _userRepository = unitOfWork.Repository<User>();

        public async Task LogAsync(
            AuditLogCategory category,
            string action,
            string? targetNric = null,
            CancellationToken cancellationToken = default)
        {
            var userId = ResolveCurrentUserId();
            if (userId.HasValue)
            {
                await LogForActorAsync(category, action, targetNric, cancellationToken: cancellationToken);
                return;
            }

            await LogAnonymousAsync(category, action, targetNric, cancellationToken: cancellationToken);
        }

        private async Task LogForActorAsync(
            AuditLogCategory category,
            string action,
            string? targetNric = null,
            string? ipAddress = null,
            CancellationToken cancellationToken = default)
        {
            await AddAsync(
                ResolveCurrentUserId(),
                category,
                action,
                targetNric,
                ipAddress,
                cancellationToken);
        }

        public async Task LogAnonymousAsync(
            AuditLogCategory category,
            string action,
            string? targetNric = null,
            string? ipAddress = null,
            CancellationToken cancellationToken = default)
        {
            await AddAsync(
                null,
                category,
                action,
                targetNric,
                ipAddress,
                cancellationToken);
        }

        private async Task AddAsync(
            int? actorUserId,
            AuditLogCategory category,
            string action,
            string? targetNric,
            string? ipAddress,
            CancellationToken cancellationToken)
        {
            var auditLog = new AuditLog
            {
                ActorUserId = actorUserId > 0 ? actorUserId : null,
                Category = category,
                Action = action,
                Nric = targetNric,
                IpAddress = ResolveIpAddress(ipAddress)
            };

            auditLog.TryValidate();
            await _auditLogRepository.AddAsync(auditLog, cancellationToken);
        }

        private int? ResolveCurrentUserId()
        {
            try
            {
                return _currentUserService.UserId;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        private string ResolveIpAddress(string? ipAddress)
        {
            return string.IsNullOrWhiteSpace(ipAddress)
                ? _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0"
                : ipAddress;
        }
    }
}
