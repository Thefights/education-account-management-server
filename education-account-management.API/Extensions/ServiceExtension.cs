using Extensions.DependencyInjection;

namespace Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBaseServices(
            this IServiceCollection services,
            AppConfiguration configuration)
        {
            services.AddResilienceServices(configuration);
            services.AddCacheServices(configuration);
            services.AddAuthServices(configuration);
            services.AddEmailServices(configuration);
            services.AddStorageServices(configuration);
            services.AddProductServices();
            services.AddSettingsServices();
            services.AddAuditServices();
            services.AddCsvServices();
            services.AddMappers();
            services.AddValidators();
            services.AddBackgroundJobServices();

            return services;
        }
    }
}
