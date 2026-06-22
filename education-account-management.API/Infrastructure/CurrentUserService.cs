using Enums;
using Infrastructure.Interface;
using Repositories.Interfaces;
using System.Security.Claims;
using Utils;

namespace Infrastructure
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor)
        : ICurrentUserService, IAuditUserContext
    {
        private readonly int _userId = ResolveUserId(httpContextAccessor.HttpContext?.User);
        private readonly string _userName = ResolveUserName(httpContextAccessor.HttpContext?.User);

        public int UserId => _userId <= 0
            ? throw new UnauthorizedAccessException("User is not authenticated.")
            : _userId;

        public int? CurrentUserId => _userId > 0 ? _userId : null;

        public UserRole Role { get; } = ResolveRole(httpContextAccessor.HttpContext?.User);

        public string UserName => _userName;

        private static int ResolveUserId(ClaimsPrincipal? user)
        {
            if (user?.Identity?.IsAuthenticated != true)
            {
                return 0;
            }

            var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdValue, out var userId) ? userId : 0;
        }

        private static UserRole ResolveRole(ClaimsPrincipal? user)
        {
            if (user?.Identity?.IsAuthenticated != true) return default;
            var roleClaim = user.FindFirstValue(ClaimTypes.Role);
            return EnumUtil.TryParseDefined<UserRole>(roleClaim, out var role) ? role : default;
        }

        private static string ResolveUserName(ClaimsPrincipal? user)
        {
            if (user?.Identity?.IsAuthenticated != true) return string.Empty;
            var userNameClaim = user.FindFirstValue(ClaimTypes.Name);
            return string.IsNullOrEmpty(userNameClaim) ? string.Empty : userNameClaim;
        }
    }
}
