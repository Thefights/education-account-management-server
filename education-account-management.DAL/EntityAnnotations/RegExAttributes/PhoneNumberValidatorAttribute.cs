namespace EntityAnnotations.RegExAttributes
{
    public class PhoneNumberValidatorAttribute() : RegularExpressionAttribute(@"^\+65[3689]\d{7}$")
    {
        public override string FormatErrorMessage(string name) =>
            $"{name} is not valid. Must start with +65 followed by 8 digits beginning with 3, 6, 8, or 9.";
    }
}
