using Common.HttpResults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Enums;
using Interfaces.Audit;
using Repositories.Interfaces;

namespace Extensions
{
    public static class SecurityExtension
    {
        private const string CorsPolicy = "AvePointMosPlatformCorsPolicy";

        public static IServiceCollection AddSecurityServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration.JwtConfig.Issuer,
                    ValidAudience = configuration.JwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.JwtConfig.SecretKey)),
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrWhiteSpace(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },

                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        if (!context.Response.HasStarted)
                        {
                            var isTokenExpired = context.AuthenticateFailure is SecurityTokenExpiredException;

                            var errorMsg = isTokenExpired
                                ? "Your session has expired. Please log in again"
                                : "You must be logged in to perform this action";

                            var result = Result.FailErrors(
                                [errorMsg],
                                errorMsg,
                                StatusCodes.Status401Unauthorized);

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            if (result is ObjectResult objectResult)
                            {
                                await context.Response.WriteAsJsonAsync(objectResult.Value);
                            }
                        }
                    },

                    OnForbidden = async context =>
                    {
                        if (!context.Response.HasStarted)
                        {
                            var result = Result.FailErrors(
                                ["You do not have permission to perform this action"],
                                "Forbidden",
                                StatusCodes.Status403Forbidden);

                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";

                            try
                            {
                                var auditLogWriter = context.HttpContext.RequestServices.GetRequiredService<IAuditLogWriter>();
                                var unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

                                await auditLogWriter.LogAsync(
                                    AuditLogCategory.Security,
                                    "ForbiddenAccessAttempt",
                                    System.Text.Json.JsonSerializer.Serialize(new
                                    {
                                        Path = context.HttpContext.Request.Path.ToString(),
                                        Method = context.HttpContext.Request.Method
                                    }),
                                    cancellationToken: context.HttpContext.RequestAborted
                                );

                                await unitOfWork.SaveChangeAsync(context.HttpContext.RequestAborted);
                            }
                            catch (Exception)
                            {
                                // Fail-safe to avoid breaking response if logging fails
                            }

                            if (result is ObjectResult objectResult)
                            {
                                await context.Response.WriteAsJsonAsync(objectResult.Value);
                            }
                        }
                    },

                    OnAuthenticationFailed = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, policy =>
                {
                    var frontendUrl = !string.IsNullOrWhiteSpace(configuration.UrlsConfig?.FrontendUrl)
                        ? configuration.UrlsConfig.FrontendUrl
                        : "http://localhost:3000";
                    var origins = new[]
                    {
                        frontendUrl,
                        "http://localhost:5173",
                        "http://localhost:5173",
                    }.Distinct().ToArray();

                    policy.WithOrigins(origins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services.AddRateLimiter(options =>
            {
                var generalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        GetRateLimitPartitionKey(context),
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = configuration.RateLimitConfig.GeneralPermitLimitPerMinute,
                            QueueLimit = configuration.RateLimitConfig.QueueLimit,
                            Window = TimeSpan.FromMinutes(1),
                            AutoReplenishment = true
                        }));

                var authMinuteLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        IsAuthSensitivePath(context.Request.Path)
                            ? $"auth-minute:{GetRateLimitPartitionKey(context)}"
                            : "auth-minute:unlimited",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = IsAuthSensitivePath(context.Request.Path)
                                ? configuration.RateLimitConfig.AuthSensitivePermitLimitPerMinute
                                : int.MaxValue,
                            QueueLimit = configuration.RateLimitConfig.QueueLimit,
                            Window = TimeSpan.FromMinutes(1),
                            AutoReplenishment = true
                        }));

                var authHourLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        IsAuthSensitivePath(context.Request.Path)
                            ? $"auth-hour:{GetRateLimitPartitionKey(context)}"
                            : "auth-hour:unlimited",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = IsAuthSensitivePath(context.Request.Path)
                                ? configuration.RateLimitConfig.AuthSensitivePermitLimitPerHour
                                : int.MaxValue,
                            QueueLimit = configuration.RateLimitConfig.QueueLimit,
                            Window = TimeSpan.FromHours(1),
                            AutoReplenishment = true
                        }));

                options.GlobalLimiter = PartitionedRateLimiter.CreateChained(
                    generalLimiter,
                    authMinuteLimiter,
                    authHourLimiter);

                options.OnRejected = async (context, cancellationToken) =>
                {
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        context.HttpContext.Response.Headers.RetryAfter =
                            ((int)Math.Ceiling(retryAfter.TotalSeconds)).ToString();
                    }

                    if (context.HttpContext.Response.HasStarted)
                    {
                        return;
                    }

                    const string message = "Too many requests. Please try again later.";
                    var result = Result.FailErrors(
                        [message],
                        message,
                        StatusCodes.Status429TooManyRequests);

                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";

                    if (result is ObjectResult objectResult)
                    {
                        await context.HttpContext.Response.WriteAsJsonAsync(
                            objectResult.Value,
                            cancellationToken);
                    }
                };
            });

            return services;
        }

        public static WebApplication UseSecurityServices(this WebApplication app)
        {
            app.UseRouting();

            app.UseCors(CorsPolicy);

            app.UseAuthentication();
            app.UseRateLimiter();
            app.UseAuthorization();

            return app;
        }

        private static string GetRateLimitPartitionKey(HttpContext context)
        {
            var authId = context.User.FindFirstValue("AuthId");
            if (!string.IsNullOrWhiteSpace(authId))
            {
                return $"auth:{authId}";
            }

            return $"ip:{context.Connection.RemoteIpAddress?.ToString() ?? "unknown"}";
        }

        private static bool IsAuthSensitivePath(PathString path)
        {
            var value = path.Value?.TrimEnd('/').ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            string[] authSensitiveSuffixes =
            [
                "/auth/register",
                "/auth/register/email-otp/send",
                "/auth/register/email-otp/verify",
                "/auth/login",
                "/auth/social-login",
                "/auth/account-holder/mock-singpass-login",
                "/auth/admin/azure-ad-login",
                "/auth/refresh-token",
                "/auth/mfa/verify",
                "/auth/mfa/resend",
                "/auth/forgot-password",
                "/auth/reset-password"
            ];

            return authSensitiveSuffixes.Any(value.EndsWith);
        }
    }
}
