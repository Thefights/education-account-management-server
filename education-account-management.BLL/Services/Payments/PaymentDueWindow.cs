namespace Services.Payments;

internal static class PaymentDueWindow
{
    public const int LeadDays = 5;

    public static DateTime GetPayableThrough(DateTime utcNow)
    {
        return utcNow.Date.AddDays(LeadDays);
    }

    public static bool IsDueForPayment(DateTime dueDate, DateTime utcNow)
    {
        return dueDate.Date <= GetPayableThrough(utcNow);
    }

    public static bool IsOverdue(DateTime dueDate, DateTime utcNow)
    {
        return dueDate.Date < utcNow.Date;
    }
}
