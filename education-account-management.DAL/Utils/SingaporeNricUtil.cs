namespace Utils;

public static class SingaporeNricUtil
{
    private static readonly int[] Weights = [2, 7, 6, 5, 4, 3, 2];
    private const string CitizenChecksums = "JZIHGFEDCBA";
    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;

        var normalized = value.Trim().ToUpperInvariant();
        if (normalized.Length != 9 || normalized[0] is not ('S' or 'T')) return false;
        if (!normalized.AsSpan(1, 7).ToString().All(char.IsDigit)) return false;

        return normalized[8] == GetChecksum(normalized[0], normalized.AsSpan(1, 7));
    }

    public static string Generate(int serialNumber)
    {
        if (serialNumber is < 0 or > 9_999_999)
        {
            throw new ArgumentOutOfRangeException(nameof(serialNumber));
        }

        var digits = serialNumber.ToString("0000000");
        return $"S{digits}{GetChecksum('S', digits)}";
    }

    private static char GetChecksum(char prefix, ReadOnlySpan<char> digits)
    {
        var sum = 0;
        for (var index = 0; index < Weights.Length; index++)
        {
            sum += (digits[index] - '0') * Weights[index];
        }

        if (prefix == 'T') sum += 4;

        return CitizenChecksums[sum % 11];
    }
}
