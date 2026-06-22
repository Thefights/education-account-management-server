using Utils;

namespace EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NumberPositiveAttribute() : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var number = TryConvertToDouble(value);
            if (number == null)
            {
                return ValidationResult.Success;
            }

            if (number < 0)
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
            return string.Format("{0} can't be negative", name.SplitWords());
        }

        private static double? TryConvertToDouble(object? value)
        {
            if (value == null)
            {
                return null;
            }

            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return null;
            }
        }
    }
}
