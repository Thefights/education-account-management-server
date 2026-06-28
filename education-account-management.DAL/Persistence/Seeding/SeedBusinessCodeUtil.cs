namespace Persistence.Seeding
{
    internal static class SeedBusinessCodeUtil
    {
        private const int SeedYear = 2026;

        public static string Generate(string prefix, int seed)
        {
            return BusinessCodeGenerator.GenerateForSeed(prefix, SeedYear, seed);
        }
    }
}
