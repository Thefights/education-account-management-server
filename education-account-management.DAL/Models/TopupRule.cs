namespace Models
{
    public class TopupRule : AuditEntity
    {
        [MessageRequired, MessageMaxLength(150), Unique]
        public string RuleName { get; set; } = string.Empty;

        [EnumDefined]
        public TopupRuleType Type { get; set; } = TopupRuleType.System;

        [EnumDefined]
        public TopupMatchMode MatchMode { get; set; } = TopupMatchMode.And;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TopupAmount { get; set; }

        [EnumDefined]
        public TopupRuleStatus Status { get; set; } = TopupRuleStatus.Active;

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<TopupRuleCondition> Conditions { get; set; } = [];

        public TopupSchedule? Schedule { get; set; }

        [OnDelete(OnDeleteBehavior.NoAction)]
        public ICollection<TopupExecution> Executions { get; set; } = [];

        [OnDelete(OnDeleteBehavior.NoAction)]
        public ICollection<TopupSystemApplication> SystemApplications { get; set; } = [];
    }
}
