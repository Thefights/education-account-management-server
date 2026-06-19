using Mappers;

namespace Extensions.DependencyInjection;

public static class MapperServiceExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddScoped<AuditLogMapper>();
        services.AddScoped<EducationAccountMapper>();
        services.AddScoped<TopupRuleMapper>();
        services.AddScoped<TopupRuleConditionMapper>();
        services.AddScoped<TopupScheduleMapper>();
        services.AddScoped<AiAssistantSettingMapper>();
        services.AddScoped<AdminManagementMapper>();
        services.AddScoped<CourseManagementMapper>();
        services.AddScoped<SchoolManagementMapper>();

        return services;
    }
}
