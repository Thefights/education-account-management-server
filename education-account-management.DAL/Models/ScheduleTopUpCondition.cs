namespace Models;

public class ScheduleTopUpCondition : BaseEntity
{
    [NotDefaultValue]
    public int GroupId { get; set; }
    public ScheduleTopUpConditionGroup Group { get; set; } = null!;

    [EnumDefined]
    public TopupConditionField Field { get; set; } = TopupConditionField.Age;

    [EnumDefined]
    public TopupConditionOperator Operator { get; set; } = TopupConditionOperator.Equals;

    [MessageMaxLength(100)]
    public string? ValueText { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ValueNumber { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ValueNumberTo { get; set; }

    public int DisplayOrder { get; set; }
}