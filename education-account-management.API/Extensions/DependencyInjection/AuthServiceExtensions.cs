using Infrastructure;
using Interfaces.Auth;
using Microsoft.Extensions.Http.Resilience;
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
            services.AddScoped<IAuthTokenService, AuthTokenService>();
            services.AddScoped<IAuthMfaService, AuthMfaService>();

            services.AddScoped<IAuthRegistrationOtpService, AuthRegistrationOtpService>();
            services.AddScoped<IAuthAccountService, AuthAccountService>();
            services.AddScoped<IAuthAccountManagementService, AuthAccountManagementService>();

            services.AddHttpClient<ISocialAuthProviderVerifier, SocialAuthProviderVerifier>()
                .AddStandardResilienceHandler(options =>
                {
                    options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(
                        configuration.ResilienceConfig.SocialProviderTimeoutSeconds);
                    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(
                        configuration.ResilienceConfig.SocialProviderTimeoutSeconds);
                    options.Retry.MaxRetryAttempts = configuration.ResilienceConfig.SocialProviderRetryCount;
                    options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(
                        configuration.ResilienceConfig.SocialProviderCircuitBreakSeconds);
                });

            return services;
        }
    }
}
