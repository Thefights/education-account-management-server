using Enums;
using Infrastructure.Interface;
using Repositories.Interfaces;
using System.Security.Claims;
using Utils;

namespace Infrastructure
{
    public class CurrentUserService : ICurrentUserService, IAuditUserContext
    {
        private readonly int _userId;
        private readonly string _userName = string.Empty;

        public int UserId => _userId <= 0
            ? throw new UnauthorizedAccessException("User is not authenticated.")
            : _userId;

        public int? CurrentUserId => _userId > 0 ? _userId : null;

        public UserRole Role { get; }

        public string UserName => _userName;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
            var user = httpContext?.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                return;
            }

            var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
            {
                return;
            }

            _userId = userId;
            var roleClaim = user.FindFirstValue(ClaimTypes.Role);
            if (EnumUtil.TryParseDefined<UserRole>(roleClaim, out var role))
            {
                Role = role;
            }

            var userNameClaim = user.FindFirstValue(ClaimTypes.Name);
            if (!string.IsNullOrEmpty(userNameClaim))
            {
                _userName = userNameClaim;
            }
        }
    }
}
