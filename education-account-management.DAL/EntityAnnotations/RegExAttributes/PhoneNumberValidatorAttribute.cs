namespace EntityAnnotations.RegExAttributes
{
    public class PhoneNumberValidatorAttribute() : RegularExpressionAttribute(@"^\+65[689]\d{7}$")
    {
        public override string FormatErrorMessage(string name) =>
            $"{name} is not valid. Must be a valid Singapore phone number in E.164 format, for example +6561234567 or +6581234567.";
    }
}
