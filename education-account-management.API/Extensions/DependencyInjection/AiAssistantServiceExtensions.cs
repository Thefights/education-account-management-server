using Interfaces;
using Services;

namespace Extensions.DependencyInjection;

public static class AiAssistantServiceExtensions
{
    public static IServiceCollection AddAiAssistantServices(this IServiceCollection services)
    {
        services.AddScoped<IAiAssistantSettingService, AiAssistantSettingService>();

        return services;
    }
}