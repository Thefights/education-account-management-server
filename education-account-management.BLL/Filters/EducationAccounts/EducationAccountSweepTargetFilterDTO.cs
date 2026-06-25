namespace Filters.EducationAccounts
{
    public class EducationAccountSweepTargetFilterDTO : FilterDTO
    {
        public List<SweepTargetStatus>? Statuses { get; set; }

        public List<SweepAction>? Actions { get; set; }

        [SearchField("Nric")]
        [FilterField(FilterOperationEnum.Contains, "Nric")]
        public string? Nric { get; set; }
    }
}
