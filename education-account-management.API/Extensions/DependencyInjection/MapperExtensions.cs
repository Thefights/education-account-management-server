using Mappers;

namespace Extensions.DependencyInjection;

public static class MapperServiceExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddScoped<EmailWhitelistMapper>();
        services.AddScoped<EmailWhitelistSettingMapper>();
        services.AddScoped<MfaSettingMapper>();
        services.AddScoped<UserMapper>();
        services.AddScoped<AuthAccountMapper>();
        services.AddScoped<AuditLogMapper>();
        services.AddScoped<ProductMapper>();

        return services;
    }
}
