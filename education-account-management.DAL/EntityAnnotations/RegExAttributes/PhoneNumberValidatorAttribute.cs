namespace EntityAnnotations.RegExAttributes
{
    public class PhoneNumberValidatorAttribute() : RegularExpressionAttribute(@"^\+[1-9]\d{1,14}$")
    {
        public override string FormatErrorMessage(string name) =>
            $"{name} is not valid. Must be a normalized international phone number in E.164 format, for example +84901234567.";
    }
}
