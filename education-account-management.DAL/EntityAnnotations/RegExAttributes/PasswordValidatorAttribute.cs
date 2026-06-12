using System.Text.RegularExpressions;

namespace EntityAnnotations.RegExAttributes
{
    public partial class PasswordValidatorAttribute : ValidationAttribute
    {
        public PasswordValidatorAttribute()
        {
            ErrorMessage = "Password must be at least 8 characters, include upper/lowercase, number and special character.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is not string password)
            {
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    [validationContext.MemberName ?? string.Empty]);
            }

            if (PlainPasswordRegex().IsMatch(password) || BcryptHashRegex().IsMatch(password))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(
                FormatErrorMessage(validationContext.DisplayName),
                [validationContext.MemberName ?? string.Empty]);
        }

        [GeneratedRegex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        private static partial Regex PlainPasswordRegex();

        [GeneratedRegex(@"^\$2[aby]\$\d{2}\$[./A-Za-z0-9]{53}$")]
        private static partial Regex BcryptHashRegex();
    }
}
