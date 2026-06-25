namespace Models
{
    public class ScheduleTopUpConditionGroup : BaseEntity
    {
        [NotDefaultValue]
        public int ScheduleTopUpId { get; set; }
        public ScheduleTopUp ScheduleTopUp { get; set; } = null!;

        public int? ParentGroupId { get; set; }
        [OnDelete(OnDeleteBehavior.NoAction)]
        public ScheduleTopUpConditionGroup? ParentGroup { get; set; }

        [EnumDefined]
        public TopupLogicalOperator LogicalOperator { get; set; } = TopupLogicalOperator.And;

        public int DisplayOrder { get; set; }

        [OnDelete(OnDeleteBehavior.NoAction)]
        public ICollection<ScheduleTopUpConditionGroup> ChildGroups { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<ScheduleTopUpCondition> Conditions { get; set; } = [];
    }
}