using System.Security.Cryptography;

namespace Services.EducationAccounts.Utils
{
    public static class EducationAccountHelper
    {
        public static string GenerateNextAccountNumber()
        {
            var prefix = $"EDU-{DateTime.UtcNow.AddHours(8).Year}-";
            var leadingLetter = RandomNumberGenerator.GetString("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 1);
            var randomPart = RandomNumberGenerator.GetString("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 6);
            return $"{prefix}{leadingLetter}{randomPart}";
        }
    }
}
