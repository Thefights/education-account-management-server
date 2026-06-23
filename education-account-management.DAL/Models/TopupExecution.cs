using System;
using System.Collections.Generic;

namespace Models
{
    public class TopupExecution : AuditEntity
    {
        [MessageRequired, MessageMaxLength(50)]
        [Unique]
        public string ExecutionCode { get; set; } = string.Empty;

        [EnumDefined]
        public TopupExecutionSourceType SourceType { get; set; } = TopupExecutionSourceType.Manual;

        public int? SystemTopupId { get; set; }
        [OnDelete(OnDeleteBehavior.NoAction)]
        public SystemTopup? SystemTopup { get; set; }

        public int? ScheduleTopUpId { get; set; }
        [OnDelete(OnDeleteBehavior.NoAction)]
        public ScheduleTopUp? ScheduleTopUp { get; set; }

        [MessageRequired, MessageMaxLength(100)]
        [Unique]
        public string IdempotencyKey { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ManualAmount { get; set; }

        [MessageMaxLength(500)]
        public string? ManualReason { get; set; }

        [EnumDefined]
        public TopupExecutionStatus Status { get; set; } = TopupExecutionStatus.Pending;

        public int TotalTargetCount { get; set; }

        public int SuccessCount { get; set; }

        public int FailedCount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalExecutedAmount { get; set; }

        [MessageMaxLength(150)]
        public string? TopupNameSnapshot { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TopupAmountSnapshot { get; set; }

        [Column(TypeName = "nvarchar(4000)"), MessageMaxLength(4000)]
        public string? ConditionsSnapshot { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<TopupExecutionTarget> Targets { get; set; } = [];
    }
}
