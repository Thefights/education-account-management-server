using Interfaces.AdminManagement;
using Services.AdminManagement;

namespace Extensions.DependencyInjection;

public static class AdminManagementServiceExtensions
{
    public static IServiceCollection AddAdminManagementServices(this IServiceCollection services)
    {
        services.AddScoped<IAdminManagementService, AdminManagementService>();
        return services;
    }
}
