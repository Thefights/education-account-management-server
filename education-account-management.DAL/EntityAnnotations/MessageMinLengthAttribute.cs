using System.ComponentModel.DataAnnotations;
using Utils;

namespace EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MessageMinLengthAttribute(int minLength) : MinLengthAttribute(minLength)
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string str && str.Length < Length)
            {
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    [validationContext.MemberName ?? string.Empty]
                );
            }

            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("{0} must be at least {1} characters", name.SplitWords(), Length);
        }
    }
}
