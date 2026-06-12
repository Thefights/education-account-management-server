using System.Text.RegularExpressions;

namespace Utils
{
    public static partial class EmailWhitelistValueUtil
    {
        public static List<string> ParseValues(string? values)
        {
            return string.IsNullOrWhiteSpace(values)
                ? []
                : values
                .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(Normalize)
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        public static string Normalize(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var normalized = value.Trim().TrimEnd('.').ToLowerInvariant();
            return IsEmail(normalized)
                ? StringUtil.NormalizeEmail(normalized)
                : normalized;
        }

        public static bool IsValid(string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                && value.Length <= 320
                && !value.Contains(';')
                && (IsEmail(value) || IsDomain(value));
        }

        private static bool IsEmail(string value)
        {
            return EmailRegex().IsMatch(value);
        }

        private static bool IsDomain(string value)
        {
            return value.Length <= 253
                && DomainRegex().IsMatch(value)
                && value.Split('.').All(label => label.Length <= 63);
        }

        [GeneratedRegex(@"^(?=.{1,320}$)(?!.*\.\.)[a-zA-Z0-9](?:[a-zA-Z0-9._%+\-]{0,62}[a-zA-Z0-9])?@[a-zA-Z0-9](?:[a-zA-Z0-9\-]{0,253}[a-zA-Z0-9])?(?:\.[a-zA-Z]{2,})+$")]
        private static partial Regex EmailRegex();

        [GeneratedRegex(@"^(?!-)(?:[a-zA-Z0-9-]{1,63}\.)+[a-zA-Z]{2,63}$")]
        private static partial Regex DomainRegex();
    }
}
