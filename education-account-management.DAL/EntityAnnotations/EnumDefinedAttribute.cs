using Utils;

namespace EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EnumDefinedAttribute() : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var type = Nullable.GetUnderlyingType(value.GetType()) ?? value.GetType();
            if (!type.IsEnum)
            {
                return new ValidationResult(
                    $"{validationContext.DisplayName} must be an enum",
                    [validationContext.MemberName ?? string.Empty]);
            }

            if (!Enum.IsDefined(type, value))
            {
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    [validationContext.MemberName ?? string.Empty]);
            }

            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("{0} must be a valid enum value", name.SplitWords());
        }
    }
}
