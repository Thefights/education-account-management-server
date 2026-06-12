namespace EntityAnnotations.RegExAttributes
{
    public class PhoneNumberValidatorAttribute : RegularExpressionAttribute
    {
        public PhoneNumberValidatorAttribute()
            : base(@"^\+[1-9]\d{1,14}$")
        {
            ErrorMessage = "{0} is not valid. Must be a normalized international phone number in E.164 format, for example +84901234567.";
        }
    }
}
