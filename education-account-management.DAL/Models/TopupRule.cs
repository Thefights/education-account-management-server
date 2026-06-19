namespace Models
{
    public class TopupRule : AuditEntity
    {
        [MessageRequired, MessageMaxLength(150)]
        public string RuleName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TopupAmount { get; set; }

        [EnumDefined]
        public TopupRuleStatus Status { get; set; } = TopupRuleStatus.Active;

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<TopupRuleCondition> Conditions { get; set; } = [];

        [OnDelete(OnDeleteBehavior.NoAction)]
        public ICollection<TopupBatch> TopupBatches { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<TopupScheduleRule> ScheduleRules { get; set; } = [];
    }
}
