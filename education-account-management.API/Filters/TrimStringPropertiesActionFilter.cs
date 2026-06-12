using Microsoft.AspNetCore.Mvc.Filters;
using Utils;

namespace Filters
{
    public class TrimStringPropertiesActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments.ToList())
            {
                if (argument.Value is string stringValue)
                {
                    context.ActionArguments[argument.Key] = string.IsNullOrWhiteSpace(stringValue)
                        ? stringValue
                        : stringValue.Trim();
                    continue;
                }

                if (argument.Value is null || IsIgnoredArgument(argument.Value))
                {
                    continue;
                }

                argument.Value.TrimStringProperties();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private static bool IsIgnoredArgument(object value)
        {
            return value is IFormFile
                || value is CancellationToken
                || value.GetType().IsValueType;
        }
    }
}
