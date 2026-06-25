namespace Filters.TopUp
{
    public sealed class ScheduleTopUpFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(ScheduleTopUp.Id),
                ["name"] = nameof(ScheduleTopUp.Name),
                ["topupAmount"] = nameof(ScheduleTopUp.TopupAmount),
                ["frequency"] = nameof(ScheduleTopUp.Frequency),
                ["status"] = nameof(ScheduleTopUp.Status),
                ["createdAt"] = nameof(ScheduleTopUp.CreatedAt)
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.In, nameof(ScheduleTopUp.Frequency))]
        public List<ScheduleTopUpFrequency>? Frequencies { get; set; }

        [FilterField(FilterOperationEnum.In, nameof(ScheduleTopUp.Status))]
        public List<ScheduleTopUpStatus>? Statuses { get; set; }

        [SearchField(nameof(ScheduleTopUp.Name))]
        public string? Name { get; set; }

        [FilterField(FilterOperationEnum.GreaterThanOrEqual, nameof(ScheduleTopUp.CreatedAt))]
        public DateTime? CreatedFrom { get; set; }

        [FilterField(FilterOperationEnum.LessThanOrEqual, nameof(ScheduleTopUp.CreatedAt))]
        public DateTime? CreatedTo { get; set; }
    }
}