namespace Filters.EducationAccounts
{
    public class EducationAccountSweepReportQueryDTO
    {
        public bool All { get; set; }

        public DateOnly? Date { get; set; }

        public DateOnly? DateFrom { get; set; }

        public DateOnly? DateTo { get; set; }
    }

    public class EducationAccountSweepTargetRangeFilterDTO : EducationAccountSweepTargetFilterDTO
    {
        public bool All { get; set; }

        public DateOnly? DateFrom { get; set; }

        public DateOnly? DateTo { get; set; }
    }
}
