using education_account_management.DAL.Exceptions;

namespace Utils
{
    public static class ObjectValidation
    {
        public static void TrimStringProperties(this object obj)
        {
            var stringProps = obj.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

            foreach (var prop in stringProps)
            {
                var value = prop.GetValue(obj) as string;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    prop.SetValue(obj, value.Trim());
                }
            }
        }

        public static void TryValidate(this Object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(obj, validationContext, validationResults, true);

            if (validationResults.Count != 0)
            {
                throw new ValidationFailureException(validationResults);
            }
        }
    }
}

