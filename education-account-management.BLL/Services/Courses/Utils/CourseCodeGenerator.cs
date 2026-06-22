using System.Security.Cryptography;

namespace Services.Courses.Utils;

public static class CourseCodeGenerator
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int SuffixLength = 7;

    public static string Generate()
    {
        Span<char> suffix = stackalloc char[SuffixLength];
        for (var index = 0; index < suffix.Length; index++)
        {
            suffix[index] = Alphabet[RandomNumberGenerator.GetInt32(Alphabet.Length)];
        }

        var singaporeYear = DateTime.UtcNow.AddHours(8).Year;
        return $"CRS-{singaporeYear}-{suffix}";
    }
}
