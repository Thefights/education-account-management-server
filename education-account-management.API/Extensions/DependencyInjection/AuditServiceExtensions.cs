using Interfaces.Audit;
using Services.Audit;

namespace Extensions.DependencyInjection;

public static class AuditServiceExtensions
{
    public static IServiceCollection AddAuditServices(this IServiceCollection services)
    {
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<IAuditLogWriter, AuditLogWriter>();

        return services;
    }
}