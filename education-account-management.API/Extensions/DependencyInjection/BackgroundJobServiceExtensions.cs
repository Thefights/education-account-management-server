using Infrastructure;
using Interfaces.Maintenance;
using Services.Maintenance;

namespace Extensions.DependencyInjection;

public static class BackgroundJobServiceExtensions
{
    public static IServiceCollection AddBackgroundJobServices(this IServiceCollection services)
    {
        services.AddScoped<IDataCleanupService, DataCleanupService>();
        services.AddHostedService<DataCleanupWorker>();

        return services;
    }
}
