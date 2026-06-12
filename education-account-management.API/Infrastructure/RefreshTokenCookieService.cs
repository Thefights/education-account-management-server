using DTOs.Auth;

namespace Infrastructure
{
    public class RefreshTokenCookieService(
        IHttpContextAccessor httpContextAccessor,
        AppConfiguration configuration)
        : IRefreshTokenCookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly AppConfiguration _configuration = configuration;

        public string? RefreshToken => _httpContextAccessor.HttpContext?.Request.Cookies[this.CookieName];

        public void Set(AuthTokenResponseDTO tokens)
        {
            this.HttpContext.Response.Cookies.Append(
                this.CookieName,
                tokens.RefreshToken,
                this.BuildCookieOptions(tokens.RefreshTokenExpiresAt));
        }

        public void Clear()
        {
            this.HttpContext.Response.Cookies.Delete(
                this.CookieName,
                this.BuildCookieOptions(DateTimeOffset.UtcNow.AddDays(-1)));
        }

        private CookieOptions BuildCookieOptions(DateTimeOffset expires)
        {
            var refreshTokenConfig = _configuration.RefreshTokenConfig;
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = refreshTokenConfig.Secure,
                SameSite = this.GetSameSiteMode(refreshTokenConfig),
                Expires = expires,
                Path = "/"
            };

            if (!string.IsNullOrWhiteSpace(refreshTokenConfig.Domain))
            {
                options.Domain = refreshTokenConfig.Domain;
            }

            return options;
        }

        private SameSiteMode GetSameSiteMode(RefreshTokenConfig refreshTokenConfig)
        {
            if (!Enum.TryParse(refreshTokenConfig.SameSite, ignoreCase: true, out SameSiteMode sameSite))
            {
                sameSite = SameSiteMode.None;
            }

            return sameSite == SameSiteMode.None && !refreshTokenConfig.Secure
                ? SameSiteMode.Lax
                : sameSite;
        }

        private HttpContext HttpContext =>
            _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is not available.");

        private string CookieName =>
            string.IsNullOrWhiteSpace(_configuration.RefreshTokenConfig.CookieName)
                ? "refreshToken"
                : _configuration.RefreshTokenConfig.CookieName;
    }
}
