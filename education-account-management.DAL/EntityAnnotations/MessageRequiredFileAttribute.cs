using Microsoft.AspNetCore.Http;

namespace EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MessageRequiredFileAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string filePathOrUrl)
            {
                if (!string.IsNullOrWhiteSpace(filePathOrUrl))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(
                    "File is required.",
                    [validationContext.MemberName ?? string.Empty]);
            }

            if (value is not IFormFile file)
            {
                return new ValidationResult(
                    "File is required.",
                    [validationContext.MemberName ?? string.Empty]);
            }

            if (file.Length == 0)
            {
                return new ValidationResult(
                    "Uploaded file cannot be empty.",
                    [validationContext.MemberName ?? string.Empty]);
            }

            return ValidationResult.Success;
        }
    }
}
