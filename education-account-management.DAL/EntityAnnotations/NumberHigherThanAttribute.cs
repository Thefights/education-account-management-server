using Utils;

namespace EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NumberHigherThanAttribute(
        double minValue,
        string? comparisonProperty,
        bool useComparisonProperty) : ValidationAttribute
    {
        public NumberHigherThanAttribute(double minValue) : this(minValue, null, false) { }

        public NumberHigherThanAttribute(string comparisonProperty) : this(0, comparisonProperty, true) { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            double? currentValue = TryConvertToDouble(value);
            if (currentValue == null)
            {
                return new ValidationResult(
                    $"{validationContext.DisplayName} must be a numeric value",
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            double comparisonValue;

            if (useComparisonProperty && comparisonProperty != null)
            {
                PropertyInfo? prop = validationContext.ObjectType.GetProperty(comparisonProperty);
                if (prop == null)
                {
                    return new ValidationResult(
                        $"Property '{comparisonProperty}' not found",
                        new[] { validationContext.MemberName ?? string.Empty }
                    );
                }

                object? otherValue = prop.GetValue(validationContext.ObjectInstance);
                double? converted = TryConvertToDouble(otherValue);
                if (converted == null)
                {
                    return new ValidationResult(
                        $"Property '{comparisonProperty}' must be numeric",
                        new[] { validationContext.MemberName ?? string.Empty }
                    );
                }

                comparisonValue = converted.Value;
            }
            else if (!useComparisonProperty)
            {
                comparisonValue = minValue;
            }
            else
            {
                return new ValidationResult(
                    "Invalid comparison configuration",
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (currentValue <= comparisonValue)
            {
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName, comparisonValue),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            return ValidationResult.Success;
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

        private string FormatErrorMessage(string name, double comparisonValue)
        {
            return string.Format("{0} must be higher than {1}", name.SplitWords(), comparisonValue);
        }
    }
}
