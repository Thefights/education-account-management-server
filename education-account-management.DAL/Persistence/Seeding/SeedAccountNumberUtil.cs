namespace Persistence.Seeding
{
    internal static class SeedAccountNumberUtil
    {
        private const string Prefix = "EDU-2026-";

        public static string Generate(int seed) => $"{Prefix}{seed:0000000}";
    }
}
