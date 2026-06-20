namespace Utils
{
    public static class MaskingHelper
    {
        public static string MaskNric(string nric)
        {
            if (string.IsNullOrWhiteSpace(nric)) return string.Empty;
            var trimmed = nric.Trim();
            if (trimmed.Length < 5) return trimmed;

            var firstChar = trimmed[0];
            var lastFour = trimmed[^4..];

            return $"{firstChar}****{lastFour}";
        }
    }
}
