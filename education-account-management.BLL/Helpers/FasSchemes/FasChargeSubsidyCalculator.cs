namespace Helpers.FasSchemes
{
    public sealed record FasChargeSubsidyResult(
        decimal SubsidyAmount,
        decimal NetAmount,
        FasApplication? AppliedApplication);

    public static class FasChargeSubsidyCalculator
    {
        public static FasChargeSubsidyResult Calculate(
            decimal courseFee,
            decimal miscFee,
            decimal taxRate,
            decimal grossAmount,
            FasApplication? application)
        {
            if (application?.ApprovedTier == null)
            {
                return new FasChargeSubsidyResult(0m, grossAmount, null);
            }

            var scheme = application.FasScheme;
            var tier = application.ApprovedTier;
            var subsidyAmount = tier.IsPerComponent
                ? CalculatePerComponent(courseFee, miscFee, taxRate, scheme.SubsidyType, tier)
                : CalculateNormal(grossAmount, scheme.SubsidyType, tier.SubsidyValue);

            subsidyAmount = RoundMoney(Math.Min(subsidyAmount, grossAmount));
            var netAmount = RoundMoney(grossAmount - subsidyAmount);
            return new FasChargeSubsidyResult(subsidyAmount, netAmount, application);
        }

        public static decimal CalculateSubsidyOnly(
            decimal courseFee,
            decimal miscFee,
            decimal taxRate,
            decimal grossAmount,
            FasApplication application)
        {
            return Calculate(courseFee, miscFee, taxRate, grossAmount, application).SubsidyAmount;
        }

        private static decimal CalculateNormal(
            decimal grossAmount,
            FasSubsidyType subsidyType,
            decimal? value)
        {
            var configuredValue = value ?? 0m;
            return subsidyType == FasSubsidyType.Percent
                ? RoundMoney(grossAmount * configuredValue / 100m)
                : RoundMoney(Math.Min(configuredValue, grossAmount));
        }

        private static decimal CalculatePerComponent(
            decimal courseFee,
            decimal miscFee,
            decimal taxRate,
            FasSubsidyType subsidyType,
            FasSchemeTier tier)
        {
            var courseGross = RoundMoney(courseFee * (1m + taxRate));
            var miscGross = RoundMoney(miscFee * (1m + taxRate));

            if (subsidyType == FasSubsidyType.Percent)
            {
                return RoundMoney(
                    courseGross * (tier.CourseFeeSubsidyValue ?? 0m) / 100m
                    + miscGross * (tier.MiscFeeSubsidyValue ?? 0m) / 100m);
            }

            return RoundMoney(
                Math.Min(tier.CourseFeeSubsidyValue ?? 0m, courseGross)
                + Math.Min(tier.MiscFeeSubsidyValue ?? 0m, miscGross));
        }

        private static decimal RoundMoney(decimal amount)
        {
            return decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
        }
    }
}
