namespace Services.Courses.Utils;

public static class CourseFeeCalculator
{
    private const decimal GstRate = 0.09m;

    public static decimal CalculateGstAmount(
        decimal courseFeeAmount,
        decimal miscFeeAmount)
    {
        var taxableAmount = courseFeeAmount + miscFeeAmount;
        return decimal.Round(
            taxableAmount * GstRate,
            2,
            MidpointRounding.AwayFromZero);
    }
}
