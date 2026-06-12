namespace Security
{
    public static class PasswordHashUtil
    {
        private const int WorkFactor = 12;
        private const string UppercaseLetters = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        private const string LowercaseLetters = "abcdefghijkmnopqrstuvwxyz";
        private const string Digits = "23456789";
        private const string SpecialCharacters = "#?!@$%^&*-";

        public static string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public static bool Verify(string password, string storedHash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, storedHash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }

        public static string GenerateTemporaryPassword(int randomLength = 12)
        {
            var randomPart = TokenUtil.GenerateRefreshToken(randomLength)[..randomLength];

            return $"{Pick(UppercaseLetters)}"
                + $"{Pick(LowercaseLetters)}"
                + $"{Pick(Digits)}"
                + $"{Pick(SpecialCharacters)}"
                + randomPart;
        }

        private static char Pick(string source)
        {
            return source[Random.Shared.Next(source.Length)];
        }
    }
}
