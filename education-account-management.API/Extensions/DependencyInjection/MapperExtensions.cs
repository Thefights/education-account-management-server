using education_account_management.BLL.Mappers;

namespace Extensions.DependencyInjection;

public static class MapperServiceExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddScoped<AuditLogMapper>();

        return services;
    }
}
