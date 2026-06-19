namespace EntityAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class SingaporeNricAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null) return ValidationResult.Success;

        return SingaporeNricUtil.IsValid(value as string)
            ? ValidationResult.Success
            : new ValidationResult(
                $"{validationContext.DisplayName} must be a valid Singapore NRIC in the S/T#######X format.",
                [validationContext.MemberName ?? validationContext.DisplayName]);
    }
}