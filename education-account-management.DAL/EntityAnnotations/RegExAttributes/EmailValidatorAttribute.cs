namespace EntityAnnotations.RegExAttributes
{
    public class EmailValidatorAttribute()
        : RegularExpressionAttribute(@"^(?=.{1,320}$)(?!.*\.\.)[a-zA-Z0-9](?:[a-zA-Z0-9._%+\-]{0,62}[a-zA-Z0-9])?@[a-zA-Z0-9](?:[a-zA-Z0-9\-]{0,253}[a-zA-Z0-9])?(?:\.[a-zA-Z]{2,})+$")
    {
        public override string FormatErrorMessage(string name) =>
            $"{name} must be in a valid format (e.g. username@example.com)";
    }
}
