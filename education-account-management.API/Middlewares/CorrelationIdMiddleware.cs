using log4net;
using System.Diagnostics;

namespace Middlewares
{
    public class CorrelationIdMiddleware(RequestDelegate _next)
    {
        private const string CorrelationIdHeaderName = "X-Correlation-ID";
        private static readonly ILog Log = LogManager.GetLogger(typeof(CorrelationIdMiddleware));

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = GetOrCreateCorrelationId(context);

            context.Response.OnStarting(() =>
            {
                context.Response.Headers.TryAdd(CorrelationIdHeaderName, correlationId);
                return Task.CompletedTask;
            });

            LogicalThreadContext.Properties["CorrelationId"] = correlationId;
            LogicalThreadContext.Properties["TraceId"] = Activity.Current?.TraceId.ToString();
            LogicalThreadContext.Properties["SpanId"] = Activity.Current?.SpanId.ToString();

            try
            {
                Log.Debug("Correlation context created.");
                await _next(context);
            }
            finally
            {
                LogicalThreadContext.Properties.Remove("CorrelationId");
                LogicalThreadContext.Properties.Remove("TraceId");
                LogicalThreadContext.Properties.Remove("SpanId");
            }
        }

        private string GetOrCreateCorrelationId(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId)
                && !string.IsNullOrEmpty(correlationId))
            {
                return correlationId!;
            }

            return Guid.NewGuid().ToString();
        }
    }
}
