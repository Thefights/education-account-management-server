namespace EntityAnnotations
{
    public sealed class PaymentPlanMonthsAttribute : ValidationAttribute
    {
        private static readonly int[] AllowedMonths = [3, 6, 9, 12];

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is not int months)
            {
                return new ValidationResult($"{validationContext.DisplayName} must be a valid number of months.");
            }

            if (!AllowedMonths.Contains(months))
            {
                return new ValidationResult($"{validationContext.DisplayName} must be one of: 3, 6, 9, 12.");
            }

            return ValidationResult.Success;
        }
    }
}
