namespace Filters.TopUp;

public sealed class TopupAccountLookupFilterDTO : FilterDTO
{
    public override int PageSize { get; set; } = 20;
}

public sealed class TopupExecutionFilterDTO : FilterDTO
{
    public override string Sort { get; set; } = "createdAt desc";
    public List<TopupExecutionSourceType>? SourceTypes { get; set; }
    public List<TopupExecutionStatus>? Statuses { get; set; }
    public string? AccountNumber { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
}

public sealed class TopupExecutionTargetFilterDTO : FilterDTO
{
    public List<TopupTargetStatus>? Statuses { get; set; }
    public string? AccountNumber { get; set; }
}
