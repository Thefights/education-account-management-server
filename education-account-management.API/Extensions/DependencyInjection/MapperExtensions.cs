using Mappers;

namespace Extensions.DependencyInjection;

public static class MapperServiceExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddScoped<AuditLogMapper>();
        services.AddScoped<AiAssistantSettingMapper>();
        services.AddScoped<AdminManagementMapper>();

        return services;
    }
}
