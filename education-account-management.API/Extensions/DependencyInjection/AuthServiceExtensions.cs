using education_account_management.BLL;
using Infrastructure;
using Infrastructure.Interface;

namespace Extensions.DependencyInjection
{
    public static class AuthServiceExtensions
    {
        public static IServiceCollection AddAuthServices(
            this IServiceCollection services,
            AppConfiguration configuration)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ICurrentTokenService, CurrentTokenService>();
            services.AddScoped<IRefreshTokenCookieService, RefreshTokenCookieService>();

            return services;
        }
    }
}

