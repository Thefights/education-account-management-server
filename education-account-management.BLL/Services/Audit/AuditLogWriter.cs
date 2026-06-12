using Interfaces.Audit;

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
        private readonly IGenericRepository<AuthAccount> _authAccountRepository = unitOfWork.Repository<AuthAccount>();

        public async Task LogAsync(
            AuditLogCategory category,
            AuditLogAction action,
            string? objectText,
            CancellationToken cancellationToken = default)
        {
            var authId = _currentUserService.AuthId;
            if (authId > 0)
            {
                var actor = await _authAccountRepository
                    .Query()
                    .Include(authAccount => authAccount.User)
                    .FirstOrDefaultAsync(authAccount => authAccount.Id == authId, cancellationToken);
                if (actor != null)
                {
                    await LogForActorAsync(actor, category, action, objectText, cancellationToken: cancellationToken);
                    return;
                }
            }

            await LogAnonymousAsync(category, action, objectText, cancellationToken: cancellationToken);
        }

        public async Task LogForActorAsync(
            AuthAccount actor,
            AuditLogCategory category,
            AuditLogAction action,
            string? objectText,
            string? ipAddress = null,
            CancellationToken cancellationToken = default)
        {
            await AddAsync(
                actor.User?.Id,
                actor.User?.FullName ?? actor.UserIdText,
                actor.UserIdText,
                category,
                action,
                objectText,
                ipAddress,
                cancellationToken);
        }

        public async Task LogAnonymousAsync(
            AuditLogCategory category,
            AuditLogAction action,
            string? objectText,
            string actorUserIdText = "Anonymous",
            string actorFullName = "Anonymous",
            string? ipAddress = null,
            CancellationToken cancellationToken = default)
        {
            await AddAsync(
                null,
                actorFullName,
                actorUserIdText,
                category,
                action,
                objectText,
                ipAddress,
                cancellationToken);
        }

        private async Task AddAsync(
            int? actorUserId,
            string actorFullName,
            string actorUserIdText,
            AuditLogCategory category,
            AuditLogAction action,
            string? objectText,
            string? ipAddress,
            CancellationToken cancellationToken)
        {
            var auditLog = new AuditLog
            {
                ActorUserId = actorUserId > 0 ? actorUserId : null,
                ActorFullName = actorFullName,
                ActorUserIdText = actorUserIdText,
                Category = category,
                Action = action,
                Object = objectText,
                IpAddress = ResolveIpAddress(ipAddress),
                CreatedAt = DateTime.UtcNow
            };

            auditLog.TryValidate();
            await _auditLogRepository.AddAsync(auditLog, cancellationToken);
        }

        private string ResolveIpAddress(string? ipAddress)
        {
            return string.IsNullOrWhiteSpace(ipAddress)
                ? _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0"
                : ipAddress;
        }
    }
}
