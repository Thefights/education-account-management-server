using System.Diagnostics;
using System.Security.Claims;

namespace Middlewares
{
    public class PerformanceMonitoringMiddleware(
        AppConfiguration configuration,
        ILogger<PerformanceMonitoringMiddleware> logger)
        : IMiddleware
    {
        private const string CorrelationIdHeaderName = "X-Correlation-ID";

        private readonly AppConfiguration _configuration = configuration;
        private readonly ILogger<PerformanceMonitoringMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (IsExcludedPath(context.Request.Path))
            {
                await next(context);
                return;
            }

            var stopwatch = Stopwatch.StartNew();
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[_configuration.PerformanceMiddlewareConfig.ResponseTimeHeaderName] =
                    stopwatch.ElapsedMilliseconds.ToString();
                return Task.CompletedTask;
            });

            await next(context);

            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var statusCode = context.Response.StatusCode;
            if (elapsedMs <= _configuration.PerformanceMiddlewareConfig.SlowRequestThresholdMs
                || statusCode >= StatusCodes.Status500InternalServerError)
            {
                return;
            }

            var user = context.User;
            _logger.LogWarning(
                "Slow API request completed. Method: {Method}, Path: {Path}, StatusCode: {StatusCode}, ElapsedMs: {ElapsedMs}, CorrelationId: {CorrelationId}, TraceId: {TraceId}, AuthId: {AuthId}, UserIdText: {UserIdText}, Role: {Role}.",
                context.Request.Method,
                context.Request.Path.Value,
                statusCode,
                elapsedMs,
                GetCorrelationId(context),
                Activity.Current?.Id ?? context.TraceIdentifier,
                user.FindFirstValue("AuthId"),
                user.FindFirstValue(ClaimTypes.Name),
                user.FindFirstValue(ClaimTypes.Role));
        }

        private bool IsExcludedPath(PathString path)
        {
            return _configuration.PerformanceMiddlewareConfig.ExcludedPathPrefixes
                .Any(prefix => path.StartsWithSegments(prefix, StringComparison.OrdinalIgnoreCase));
        }

        private static string? GetCorrelationId(HttpContext context)
        {
            return context.Response.Headers.TryGetValue(CorrelationIdHeaderName, out var responseCorrelationId)
                ? responseCorrelationId.ToString()
                : context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var requestCorrelationId)
                    ? requestCorrelationId.ToString()
                    : null;
        }
    }
}
