using System.Security.Cryptography;

namespace Services.Courses.Utils;

public static class CourseCodeGenerator
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int SuffixLength = 7;
    private const int MaxGenerationAttempts = 10;

    public static string Generate(DateTime utcNow)
    {
        Span<char> suffix = stackalloc char[SuffixLength];
        for (var index = 0; index < suffix.Length; index++)
        {
            suffix[index] = Alphabet[RandomNumberGenerator.GetInt32(Alphabet.Length)];
        }

        var singaporeYear = utcNow.AddHours(8).Year;
        return $"CRS-{singaporeYear}-{suffix}";
    }

    public static async Task<string> GenerateUniqueAsync(
        IGenericRepository<Course> courseRepository,
        int schoolId,
        DateTime utcNow,
        ISet<string>? reservedCodes = null,
        CancellationToken cancellationToken = default)
    {
        for (var attempt = 0; attempt < MaxGenerationAttempts; attempt++)
        {
            var courseCode = Generate(utcNow);
            if (reservedCodes?.Contains(courseCode) == true)
            {
                continue;
            }

            if (!await courseRepository.AnyAsync(
                    course => course.SchoolId == schoolId
                        && course.CourseCode == courseCode,
                    cancellationToken))
            {
                reservedCodes?.Add(courseCode);
                return courseCode;
            }
        }

        throw new DataConflictException("Unable to generate a unique course code.");
    }
}