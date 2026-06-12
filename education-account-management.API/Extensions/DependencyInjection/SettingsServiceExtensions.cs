using Interfaces.Email;
using Services.Email;

namespace Extensions.DependencyInjection;

public static class SettingsServiceExtensions
{
    public static IServiceCollection AddSettingsServices(this IServiceCollection services)
    {
        services.AddScoped<IMfaSettingService, MfaSettingService>();
        services.AddScoped<IEmailWhitelistService, EmailWhitelistService>();
        services.AddScoped<IEmailWhitelistSettingService, EmailWhitelistSettingService>();

        return services;
    }
}
