namespace Models;

public class SystemTopupConditionGroup : BaseEntity
{
    [NotDefaultValue]
    public int SystemTopupId { get; set; }
    public SystemTopup SystemTopup { get; set; } = null!;

    public int? ParentGroupId { get; set; }
    [OnDelete(OnDeleteBehavior.NoAction)]
    public SystemTopupConditionGroup? ParentGroup { get; set; }

    [EnumDefined]
    public TopupLogicalOperator LogicalOperator { get; set; } = TopupLogicalOperator.And;

    public int DisplayOrder { get; set; }

    [OnDelete(OnDeleteBehavior.NoAction)]
    public ICollection<SystemTopupConditionGroup> ChildGroups { get; set; } = [];

    [OnDelete(OnDeleteBehavior.Cascade)]
    public ICollection<SystemTopupCondition> Conditions { get; set; } = [];
}