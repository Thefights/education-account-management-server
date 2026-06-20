namespace EntityAnnotations.RegExAttributes
{
    public class OtpValidatorAttribute() : RegularExpressionAttribute(@"^\d{6}$")
    {
        public override string FormatErrorMessage(string name) => "OTP must be exactly 6 digits.";
    }
}
