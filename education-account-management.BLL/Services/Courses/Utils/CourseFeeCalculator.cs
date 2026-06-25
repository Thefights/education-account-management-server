namespace Services.Courses.Utils
{
    public static class CourseFeeCalculator
    {
        public static decimal CalculateTaxAmount(
            decimal courseFeeAmount,
            decimal miscFeeAmount,
            decimal taxRate)
        {
            var taxableAmount = courseFeeAmount + miscFeeAmount;
            return decimal.Round(
                taxableAmount * taxRate,
                2,
                MidpointRounding.AwayFromZero);
        }
    }
}
