using DTOs.Auth;
using Validators;

namespace Extensions.DependencyInjection;

public static class ValidatorServiceExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<RegisterRequestDTO>, RegisterRequestValidator>();
        services.AddScoped<IValidator<SendRegisterEmailOtpRequestDTO>, SendRegisterEmailOtpRequestValidator>();
        services.AddScoped<IValidator<VerifyRegisterEmailOtpRequestDTO>, VerifyRegisterEmailOtpRequestValidator>();
        services.AddScoped<IValidator<LoginRequestDTO>, LoginRequestValidator>();
        services.AddScoped<IValidator<SocialLoginRequestDTO>, SocialLoginRequestValidator>();
        services.AddScoped<IValidator<ResendMfaOtpRequestDTO>, ResendMfaOtpRequestValidator>();
        services.AddScoped<IValidator<VerifyMfaOtpRequestDTO>, VerifyMfaOtpRequestValidator>();
        services.AddScoped<IValidator<ResetPasswordRequestDTO>, ResetPasswordRequestValidator>();

        return services;
    }
}