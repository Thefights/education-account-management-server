namespace Filters.EducationAccounts
{
    public class EducationAccountSweepTargetFilterDTO : FilterDTO
    {
        public SweepTargetStatus? Status { get; set; }

        public SweepAction? Action { get; set; }

        [SearchField("Nric")]
        [FilterField(FilterOperationEnum.Contains, "Nric")]
        public string? Nric { get; set; }
    }
}
