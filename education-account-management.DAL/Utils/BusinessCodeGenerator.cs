using Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace Utils
{
    public static class BusinessCodeGenerator
    {
        public const int SuffixLength = 7;
        public const int MaxGenerationAttempts = 1000;

        public const string StaffPrefix = "STAFF";
        public const string EducationAccountPrefix = "EDU";
        public const string CoursePrefix = "CRS";
        public const string FasSchemePrefix = "FAS";
        public const string FasApplicationPrefix = "FASAPP";

        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string Generate(string prefix)
        {
            return Generate(prefix, DateTime.UtcNow);
        }

        public static string Generate(string prefix, DateTime utcNow)
        {
            var normalizedPrefix = NormalizePrefix(prefix);
            Span<char> suffix = stackalloc char[SuffixLength];
            for (var index = 0; index < suffix.Length; index++)
            {
                suffix[index] = Alphabet[RandomNumberGenerator.GetInt32(Alphabet.Length)];
            }

            return $"{normalizedPrefix}-{utcNow.Year}-{suffix}";
        }

        public static async Task<string> GenerateUniqueAsync(
            string prefix,
            Func<string, CancellationToken, Task<bool>> existsAsync,
            DateTime? utcNow = null,
            ISet<string>? reservedCodes = null,
            string? conflictMessage = null,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(existsAsync);

            for (var attempt = 0; attempt < MaxGenerationAttempts; attempt++)
            {
                var code = utcNow.HasValue
                    ? Generate(prefix, utcNow.Value)
                    : Generate(prefix);

                if (reservedCodes?.Contains(code) == true)
                {
                    continue;
                }

                if (!await existsAsync(code, cancellationToken))
                {
                    reservedCodes?.Add(code);
                    return code;
                }
            }

            throw new DataConflictException(conflictMessage ?? $"Unable to generate a unique {NormalizePrefix(prefix)} code.");
        }

        public static string GenerateForSeed(string prefix, int year, int seed)
        {
            if (seed < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(seed), "Seed must be non-negative.");
            }

            var normalizedPrefix = NormalizePrefix(prefix);
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes($"{normalizedPrefix}:{year}:{seed}"));
            Span<char> suffix = stackalloc char[SuffixLength];
            for (var index = 0; index < suffix.Length; index++)
            {
                suffix[index] = Alphabet[hash[index] % Alphabet.Length];
            }

            return $"{normalizedPrefix}-{year}-{suffix}";
        }

        private static string NormalizePrefix(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Prefix is required.", nameof(prefix));
            }

            var normalizedPrefix = prefix.Trim().ToUpperInvariant();
            if (normalizedPrefix.Any(character => !char.IsAsciiLetterOrDigit(character)))
            {
                throw new ArgumentException("Prefix can only contain letters and numbers.", nameof(prefix));
            }

            return normalizedPrefix;
        }
    }
}
