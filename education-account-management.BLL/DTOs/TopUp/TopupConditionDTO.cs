namespace DTOs.TopUp;

public sealed class TopupConditionDTO
{
    public int Id { get; set; }
    public string? Field { get; set; }
    public string? Operator { get; set; }
    public string? ValueText { get; set; }
    public decimal? ValueNumber { get; set; }
    public decimal? ValueNumberTo { get; set; }
    public int DisplayOrder { get; set; }
}

public sealed class TopupConditionGroupDTO
{
    public int Id { get; set; }
    public string? LogicalOperator { get; set; }
    public int DisplayOrder { get; set; }
    public List<TopupConditionDTO> Conditions { get; set; } = [];
    public List<TopupConditionGroupDTO> Groups { get; set; } = [];
}

public sealed class TopupConditionRequestDTO
{
    [EnumDefined]
    public TopupConditionField Field { get; set; }

    [EnumDefined]
    public TopupConditionOperator Operator { get; set; }

    public string? ValueText { get; set; }
    public decimal? ValueNumber { get; set; }
    public decimal? ValueNumberTo { get; set; }
    public int DisplayOrder { get; set; }
}

public sealed class TopupConditionGroupRequestDTO
{
    [EnumDefined]
    public TopupLogicalOperator LogicalOperator { get; set; }

    public int DisplayOrder { get; set; }
    public List<TopupConditionRequestDTO> Conditions { get; set; } = [];
    public List<TopupConditionGroupRequestDTO> Groups { get; set; } = [];
}