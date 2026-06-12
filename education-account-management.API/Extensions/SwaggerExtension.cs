using NSwag;
using NSwag.Generation.Processors.Security;
using System.Reflection;

namespace Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddOpenApiDocument(options =>
            {
                options.Title = configuration.AppInfo?.Name
                    ?? Assembly.GetEntryAssembly()?.GetName().Name
                    ?? "avepoint-mos-platform API";
                options.Version = configuration.AppInfo?.Version
                    ?? Assembly.GetEntryAssembly()?.GetName().Version?.ToString()
                    ?? "v1";

                options.AddSecurity("Bearer", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Enter a valid JWT bearer token."
                });

                options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
            });

            return services;
        }

        public static WebApplication UseSwaggerServices(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi();
            }

            return app;
        }
    }
}
