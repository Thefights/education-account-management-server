using Middlewares;

namespace Extensions
{
    public static class MiddlewareExtension
    {
        public static IServiceCollection AddMiddlewares(this IServiceCollection services)
        {
            services.AddScoped<ExceptionHandlingMiddleware>();
            services.AddScoped<PerformanceMonitoringMiddleware>();

            return services;
        }

        public static WebApplication UseMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<PerformanceMonitoringMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<JwtBlacklistMiddleware>();
            app.UseMiddleware<ApiKeyMiddleware>();

            return app;
        }
    }
}
