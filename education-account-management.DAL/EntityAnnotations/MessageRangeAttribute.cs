using Utils;

namespace EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MessageRangeAttribute(double minimum, double maximum) : RangeAttribute(minimum, maximum)
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var number = TryConvertToDouble(value);
            if (number == null)
            {
                return ValidationResult.Success;
            }

            var configuredMinimum = Convert.ToDouble(Minimum);
            var configuredMaximum = Convert.ToDouble(Maximum);
            if (number < configuredMinimum || number > configuredMaximum)
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
            return string.Format("{0} must be between {1} and {2}", name.SplitWords(), Minimum, Maximum);
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
