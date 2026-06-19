using Infrastructure;
using Infrastructure.Interface;
using Interfaces.Auth;
using Repositories.Interfaces;
using Services.Auth;

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
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<CurrentUserService>();
            services.AddScoped<ICurrentUserService>(provider =>
                provider.GetRequiredService<CurrentUserService>());
            services.AddScoped<IAuditUserContext>(provider =>
                provider.GetRequiredService<CurrentUserService>());

            return services;
        }
    }
}

