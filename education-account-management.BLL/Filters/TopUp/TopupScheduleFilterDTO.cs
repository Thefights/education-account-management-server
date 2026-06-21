namespace Filters.TopUp
{
    public class TopupScheduleFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> CustomSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = "Id",
                ["frequency"] = "Frequency",
                ["status"] = "Status",
                ["createdAt"] = "CreatedAt"
            };

        public override IReadOnlyDictionary<string, string> SortFields => CustomSortFields;

        [FilterField(FilterOperationEnum.In, "Frequency")]
        public List<TopupScheduleType>? Frequencies { get; set; }

        [FilterField(FilterOperationEnum.In, "Status")]
        public List<TopupScheduleStatus>? Statuses { get; set; }

        [FilterField(FilterOperationEnum.GreaterThanOrEqual, "CreatedAt")]
        public DateTime? CreatedFrom { get; set; }

        [FilterField(FilterOperationEnum.LessThanOrEqual, "CreatedAt")]
        public DateTime? CreatedTo { get; set; }

        [FilterField(FilterOperationEnum.Equal, "TopupRuleId")]
        public int? TopupRuleId { get; set; }
    }
}
