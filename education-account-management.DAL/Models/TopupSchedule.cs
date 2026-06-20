using System;

namespace Models
{
    public class TopupSchedule : AuditEntity
    {
        [NotDefaultValue]
        public int TopupRuleId { get; set; }
        public TopupRule TopupRule { get; set; } = null!;

        [EnumDefined]
        public TopupScheduleType Frequency { get; set; } = TopupScheduleType.OneTime;

        public DateTime? OneTimeExecutionAt { get; set; }

        [MessageRange(1, 31)]
        public int? ExecuteAtDay { get; set; }

        [MessageRange(1, 12)]
        public int? ExecuteAtMonth { get; set; }

        public TimeOnly ExecutionTime { get; set; } = TimeOnly.MinValue;

        public DateTime? NextExecutionAt { get; set; }

        [EnumDefined]
        public TopupScheduleStatus Status { get; set; } = TopupScheduleStatus.Active;

        [OnDelete(OnDeleteBehavior.NoAction)]
        public ICollection<TopupExecution> Executions { get; set; } = [];
    }
}
