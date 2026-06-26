namespace Persistence.Seeding
{
    internal static class SeedAccountNumberUtil
    {
        private const string Prefix = "EDU-2026-";
        private const string Letters = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        private const string Digits = "23456789";
        private const string AlphaNumeric = Letters + Digits;

        public static string Generate(int seed)
        {
            var hash = Mix((uint)seed);
            var suffix = new char[7];

            for (var index = 0; index < suffix.Length; index++)
            {
                hash = Mix(hash + (uint)index + 17u);
                suffix[index] = AlphaNumeric[(int)(hash % AlphaNumeric.Length)];
            }

            var letterIndex = (int)(hash % suffix.Length);
            var digitIndex = (int)(Mix(hash + 47u) % (suffix.Length - 1));
            if (digitIndex >= letterIndex)
            {
                digitIndex++;
            }

            suffix[letterIndex] = Letters[(int)(Mix(hash + 31u) % Letters.Length)];
            suffix[digitIndex] = Digits[(int)(Mix(hash + 59u) % Digits.Length)];

            return $"{Prefix}{new string(suffix)}";
        }

        private static uint Mix(uint value)
        {
            value ^= value >> 16;
            value *= 0x7feb352d;
            value ^= value >> 15;
            value *= 0x846ca68b;
            value ^= value >> 16;
            return value;
        }
    }
}
