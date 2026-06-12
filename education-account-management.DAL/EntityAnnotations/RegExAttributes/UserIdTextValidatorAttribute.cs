using System.Text.RegularExpressions;

namespace EntityAnnotations.RegExAttributes
{
    public class UserIdTextValidatorAttribute : ValidationAttribute
    {
        private static readonly Regex Pattern = new(
            @"^[A-Za-z0-9 +\-_\.@]+$",
            RegexOptions.Compiled);

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string userIdText)
            {
                return ValidationResult.Success;
            }

            if (userIdText.Length is < 6 or > 256)
            {
                return new ValidationResult(
                    "User ID must be between 6 and 256 characters.",
                    [validationContext.MemberName ?? string.Empty]);
            }

            if (!Pattern.IsMatch(userIdText))
            {
                return new ValidationResult(
                    "User ID can contain letters, digits, spaces, and + - _ . @ only.",
                    [validationContext.MemberName ?? string.Empty]);
            }

            return ValidationResult.Success;
        }
    }
}
