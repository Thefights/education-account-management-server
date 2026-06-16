using Enums;
using Infrastructure.Interface;
using System.Security.Claims;
using Utils;

namespace Infrastructure
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly int _userId;

        public int UserId => _userId <= 0
            ? throw new UnauthorizedAccessException("User is not authenticated.")
            : _userId;

        public int AuthId { get; }

        public UserRole Role { get; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
            var user = httpContext?.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                return;
            }

            var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var authIdValue = user.FindFirstValue("AuthId");

            if (!int.TryParse(userIdValue, out var userId) || !int.TryParse(authIdValue, out var authId))
            {
                return;
            }

            _userId = userId;
            AuthId = authId;

            var roleClaim = user.FindFirstValue(ClaimTypes.Role);
            if (EnumUtil.TryParseDefined<UserRole>(roleClaim, out var role))
            {
                Role = role;
            }
        }
    }
}
