namespace Filters.EducationAccounts
{
    public class EducationAccountSweepTargetFilterDTO : FilterDTO
    {
        public DateOnly? BatchDate { get; set; }

        public SweepTargetStatus? Status { get; set; }

        [SearchField("Nric")]
        [FilterField(FilterOperationEnum.Contains, "Nric")]
        public string? Nric { get; set; }
    }
}
