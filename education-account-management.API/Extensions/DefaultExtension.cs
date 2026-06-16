using Common.HttpResults;
using Filters;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text.Json.Serialization;

namespace Extensions
{
    public static class DefaultExtension
    {
        public static IServiceCollection AddDefaultAPIServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllers(options =>
            {
                options.Conventions.Add(new AutoBindingConvention());
                options.Conventions.Add(
                    new RouteTokenTransformerConvention(new DashCaseParameterTransformer()));
                options.Filters.Add<TrimStringPropertiesActionFilter>();
            })
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var fieldErrors = context.ModelState
                        .Where(kv => kv.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kv => kv.Key,
                            kv =>
                            {
                                var e = kv.Value!.Errors.First();
                                return string.IsNullOrWhiteSpace(e.ErrorMessage)
                                    ? e.Exception?.Message ?? "Invalid"
                                    : e.ErrorMessage;
                            });

                    return Result.FailFieldErrors(fieldErrors, "Validation failed", StatusCodes.Status400BadRequest);
                };
            });

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version")
                );
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        public static WebApplication UseDefaultAPIServices(this WebApplication app)
        {
            app.MapGet("/health", () => TypedResults.Ok(new
            {
                status = "Healthy",
                timestamp = DateTimeOffset.UtcNow
            }))
            .AllowAnonymous();

            app.MapControllers();

            return app;
        }
    }
}

