using Interfaces;
using Services;

namespace Extensions.DependencyInjection;

public static class SchoolManagementServiceExtensions
{
    public static IServiceCollection AddSchoolManagementServices(this IServiceCollection services)
    {
        services.AddScoped<ISchoolManagementService, SchoolManagementService>();
        return services;
    }
}