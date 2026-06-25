namespace EntityAnnotations
{
    public sealed class DurationInMonthsAttribute : ValidationAttribute
    {
        private const int MinimumMonths = 1;

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

            if (months < MinimumMonths)
            {
                return new ValidationResult($"{validationContext.DisplayName} must be at least {MinimumMonths} month.");
            }

            return ValidationResult.Success;
        }
    }
}
