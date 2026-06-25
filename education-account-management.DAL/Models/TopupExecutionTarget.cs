using System;

namespace Models
{
    public class TopupExecutionTarget : AuditEntity
    {
        [NotDefaultValue]
        public int TopupExecutionId { get; set; }
        public TopupExecution TopupExecution { get; set; } = null!;

        public int? EducationAccountId { get; set; }
        [OnDelete(OnDeleteBehavior.NoAction)]
        public EducationAccount? EducationAccount { get; set; }

        [MessageRequired, MessageMaxLength(20)]
        public string AccountNumber { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [EnumDefined]
        public TopupTargetStatus Status { get; set; } = TopupTargetStatus.Pending;

        [MessageMaxLength(500)]
        public string? FailureReason { get; set; }

        [Column(TypeName = "nvarchar(4000)"), MessageMaxLength(4000)]
        public string? MatchedConditionsSnapshot { get; set; }

        [Unique]
        public int? EducationCreditTransactionId { get; set; }
        [OnDelete(OnDeleteBehavior.NoAction)]
        public EducationCreditTransaction? EducationCreditTransaction { get; set; }
    }
}
