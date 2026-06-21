namespace Filters.TopUp;

public sealed class TopupExecutionFilterDTO : FilterDTO
{
    private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["id"] = "Id",
            ["executionCode"] = "ExecutionCode",
            ["sourceType"] = "SourceType",
            ["status"] = "Status",
            ["totalExecutedAmount"] = "TotalExecutedAmount",
            ["createdAt"] = "CreatedAt"
        };

    public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;
    public List<TopupExecutionSourceType>? SourceTypes { get; set; }
    public List<TopupExecutionStatus>? Statuses { get; set; }
    public string? AccountNumber { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
}

public sealed class TopupExecutionTargetFilterDTO : FilterDTO
{
    private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["id"] = "Id",
            ["accountNumber"] = "AccountNumber",
            ["amount"] = "Amount",
            ["status"] = "Status",
            ["createdAt"] = "CreatedAt"
        };

    public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;
    public List<TopupTargetStatus>? Statuses { get; set; }
    public string? AccountNumber { get; set; }
}
