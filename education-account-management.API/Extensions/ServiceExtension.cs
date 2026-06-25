using Extensions.DependencyInjection;

namespace Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBaseServices(
            this IServiceCollection services,
            AppConfiguration configuration)
        {
            services.AddApplicationServices(configuration);

            return services;
        }
    }
}
