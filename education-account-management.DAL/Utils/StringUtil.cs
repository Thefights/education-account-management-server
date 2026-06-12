using System.Text.RegularExpressions;

namespace Utils
{
    public static class StringUtil
    {
        public static string? SplitWords(this string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? value
                : Regex.Replace(
                value,
                @"(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])",
                " "
            );
        }

        public static string NormalizeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;

            var lowerEmail = email.ToLowerInvariant();
            var parts = lowerEmail.Split('@');
            if (parts.Length != 2)
                return lowerEmail;

            var username = parts[0];
            var domain = parts[1];

            if (domain is "gmail.com" or "googlemail.com")
            {
                var plusIndex = username.IndexOf('+');
                if (plusIndex != -1)
                {
                    username = username.Substring(0, plusIndex);
                }

                username = username.Replace(".", "");
            }

            return $"{username}@{domain}";
        }

        public static string Truncate(string value, int maxLength)
        {
            return value.Length <= maxLength
                ? value
                : value[..maxLength];
        }
    }
}
