namespace DTOs.Dashboards
{
    public sealed class DashboardDateRangeDTO
    {
        public int? RangeDays { get; set; }

        public DateOnly? DateFrom { get; set; }

        public DateOnly? DateTo { get; set; }
    }
}
