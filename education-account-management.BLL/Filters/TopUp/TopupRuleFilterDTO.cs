namespace Filters.TopUp;

public sealed class TopupRuleFilterDTO : FilterDTO
{
    private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["id"] = nameof(TopupRule.Id),
            ["ruleName"] = nameof(TopupRule.RuleName),
            ["type"] = nameof(TopupRule.Type),
            ["matchMode"] = nameof(TopupRule.MatchMode),
            ["topupAmount"] = nameof(TopupRule.TopupAmount),
            ["status"] = nameof(TopupRule.Status),
            ["createdAt"] = nameof(TopupRule.CreatedAt)
        };

    public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

    [FilterField(FilterOperationEnum.In, nameof(TopupRule.Type))]
    public List<TopupRuleType>? Types { get; set; }

    [FilterField(FilterOperationEnum.In, nameof(TopupRule.Status))]
    public List<TopupRuleStatus>? Statuses { get; set; }

    [SearchField(nameof(TopupRule.RuleName))]
    public string? RuleName { get; set; }
}
