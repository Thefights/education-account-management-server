namespace Services.TopUp
{
    internal static class TopupEligibilityPolicy
    {
        public const int MinimumAge = 16;
        public const int MaximumAge = 30;
        public const decimal MaximumBalance = 9999999999999999.99m;

        public static bool IsEligibleAge(int age) => age is >= MinimumAge and <= MaximumAge;
    }
}
