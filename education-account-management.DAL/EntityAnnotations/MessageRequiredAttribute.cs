using Utils;

namespace EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MessageRequiredAttribute() : RequiredAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
            {
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName, validationContext.ObjectType.Name),
                    [validationContext.MemberName ?? string.Empty]
                );
            }

            return ValidationResult.Success;
        }

        public string FormatErrorMessage(string fieldName, string className)
        {
            return string.Format("{0} in {1} is required", fieldName.SplitWords(), className.SplitWords());
        }
    }
}
