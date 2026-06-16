using Infrastructure.Interface;

namespace Infrastructure
{
    public class CurrentTokenService(IHttpContextAccessor httpContextAccessor) : ICurrentTokenService
    {
        public string? AccessToken { get; } = GetBearerToken(httpContextAccessor.HttpContext);

        private static string? GetBearerToken(HttpContext? httpContext)
        {
            var authorizationHeader = httpContext?.Request.Headers.Authorization.ToString();
            return authorizationHeader?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true
                ? authorizationHeader[7..]
                : null;
        }
    }
}
