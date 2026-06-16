using Infrastructure.Interface;

namespace Infrastructure
{
    public class RefreshTokenCookieService(
        IHttpContextAccessor httpContextAccessor,
        AppConfiguration configuration)
        : IRefreshTokenCookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly RefreshTokenConfig _config = configuration.RefreshTokenConfig;

        public string? Get()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies[_config.CookieName];
        }

        public void Set(string refreshToken, DateTimeOffset? expires = null)
        {
            var response = _httpContextAccessor.HttpContext?.Response;
            if (response == null)
            {
                return;
            }

            response.Cookies.Append(
                _config.CookieName,
                refreshToken,
                BuildCookieOptions(expires ?? DateTimeOffset.UtcNow.AddDays(_config.ExpirationDays)));
        }

        public void Clear()
        {
            var response = _httpContextAccessor.HttpContext?.Response;
            if (response == null)
            {
                return;
            }

            response.Cookies.Delete(_config.CookieName, BuildCookieOptions(DateTimeOffset.UnixEpoch));
        }

        private CookieOptions BuildCookieOptions(DateTimeOffset expires)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = _config.Secure,
                SameSite = ParseSameSiteMode(_config.SameSite),
                Expires = expires,
                Path = "/",
                IsEssential = true
            };

            if (!string.IsNullOrWhiteSpace(_config.Domain))
            {
                options.Domain = _config.Domain;
            }

            return options;
        }

        private static SameSiteMode ParseSameSiteMode(string? sameSite)
        {
            return Enum.TryParse<SameSiteMode>(sameSite, ignoreCase: true, out var mode)
                ? mode
                : SameSiteMode.Lax;
        }
    }
}
