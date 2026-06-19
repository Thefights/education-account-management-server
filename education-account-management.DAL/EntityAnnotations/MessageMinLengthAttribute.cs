using System.ComponentModel.DataAnnotations;
using Utils;

namespace EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MessageMinLengthAttribute : MinLengthAttribute
    {
        private readonly int _minLength;

        public MessageMinLengthAttribute(int minLength) : base(minLength)
        {
            _minLength = minLength;
            ErrorMessage = "{0} must be at least {1} characters";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string str && str.Length < _minLength)
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
            return string.Format(ErrorMessageString, name.SplitWords(), _minLength);
        }
    }
}
