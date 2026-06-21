namespace Filters.TopUp;

public sealed class TopupRuleFilterDTO : FilterDTO
{
    private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["id"] = nameof(TopupRule.Id),
            ["ruleName"] = nameof(TopupRule.RuleName),
            ["type"] = nameof(TopupRule.Type),
            ["status"] = nameof(TopupRule.Status),
            ["createdAt"] = nameof(TopupRule.CreatedAt)
        };

    public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

    [FilterField(FilterOperationEnum.Equal, nameof(TopupRule.Type))]
    public TopupRuleType? Type { get; set; }

    [FilterField(FilterOperationEnum.Equal, nameof(TopupRule.Status))]
    public TopupRuleStatus? Status { get; set; }

    [SearchField(nameof(TopupRule.RuleName))]
    public string? RuleName { get; set; }
}
