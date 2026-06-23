namespace Models
{
    public class ScheduleTopUp : AuditEntity
    {
        [MessageRequired, MessageMaxLength(150), Unique]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TopupAmount { get; set; }

        [EnumDefined]
        public ScheduleTopUpFrequency Frequency { get; set; } = ScheduleTopUpFrequency.OneTime;

        public DateTime? OneTimeExecutionAt { get; set; }

        [MessageRange(1, 31)]
        public int? ExecuteAtDay { get; set; }

        [MessageRange(1, 12)]
        public int? ExecuteAtMonth { get; set; }

        public TimeOnly ExecutionTime { get; set; } = TimeOnly.MinValue;

        public DateTime? NextExecutionAt { get; set; }

        [EnumDefined]
        public ScheduleTopUpStatus Status { get; set; } = ScheduleTopUpStatus.Active;

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<ScheduleTopUpConditionGroup> ConditionGroups { get; set; } = [];

        [OnDelete(OnDeleteBehavior.NoAction)]
        public ICollection<TopupExecution> Executions { get; set; } = [];
    }
}