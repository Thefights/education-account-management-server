using Interfaces;
using Services;

namespace Extensions.DependencyInjection;

public static class CourseManagementServiceExtensions
{
    public static IServiceCollection AddCourseManagementServices(this IServiceCollection services)
    {
        services.AddScoped<ICourseManagementService, CourseManagementService>();
        return services;
    }
}