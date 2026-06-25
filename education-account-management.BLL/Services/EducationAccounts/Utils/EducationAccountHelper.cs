using System;
using System.Security.Cryptography;

namespace Services.EducationAccounts.Utils
{
    public static class EducationAccountHelper
    {
        public static string GenerateNextAccountNumber()
        {
            var prefix = $"EDU-{DateTime.UtcNow.AddHours(8).Year}-";
            var randomPart = RandomNumberGenerator.GetString("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 11);
            return $"{prefix}{randomPart}";
        }
    }
}
