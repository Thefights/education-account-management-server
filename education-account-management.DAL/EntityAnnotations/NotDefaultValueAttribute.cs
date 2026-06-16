using Utils;

namespace EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotDefaultValueAttribute : ValidationAttribute
    {
        public NotDefaultValueAttribute()
        {
            ErrorMessage = "{0} cannot be default value";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var type = Nullable.GetUnderlyingType(value.GetType()) ?? value.GetType();
            var defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;

            if (Equals(value, defaultValue))
            {
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    [validationContext.MemberName ?? string.Empty]);
            }

            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name.SplitWords());
        }
    }
}
