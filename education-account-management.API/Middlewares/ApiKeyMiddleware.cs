namespace Middlewares
{
    public class ApiKeyMiddleware(RequestDelegate next)
    {
        private const string ApiKey = "ApiKey ";

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var hasAuthorize = endpoint?.Metadata.GetMetadata<IAuthorizeData>() != null;

            if (!hasAuthorize)
            {
                await next(context);
                return;
            }

            var authHeader = context.Request.Headers.Authorization.ToString();

            if (!authHeader.StartsWith(ApiKey, StringComparison.OrdinalIgnoreCase))
            {
                await next(context);
                return;
            }

            await next(context);
        }
    }
}
