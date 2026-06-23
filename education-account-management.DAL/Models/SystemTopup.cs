namespace Models
{
    public class SystemTopup : AuditEntity
    {
        [MessageRequired, MessageMaxLength(150), Unique]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TopupAmount { get; set; }

        [EnumDefined]
        public SystemTopupStatus Status { get; set; } = SystemTopupStatus.Active;

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<SystemTopupConditionGroup> ConditionGroups { get; set; } = [];

        [OnDelete(OnDeleteBehavior.NoAction)]
        public ICollection<TopupExecution> Executions { get; set; } = [];

        [OnDelete(OnDeleteBehavior.NoAction)]
        public ICollection<TopupSystemApplication> Applications { get; set; } = [];
    }
}