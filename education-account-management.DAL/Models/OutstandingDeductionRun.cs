namespace Models
{
    public class OutstandingDeductionRun : AuditEntity
    {
        [MessageRequired, MessageMaxLength(7), RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])$")]
        public string RunMonth { get; set; } = string.Empty;

        [NotDefaultValue]
        public DateOnly RunDate { get; set; }

        [EnumDefined]
        public OutstandingDeductionRunStatus Status { get; set; } = OutstandingDeductionRunStatus.Running;

        [NumberPositive]
        public int TotalScannedCharges { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal TotalDeductedAmount { get; set; }

        [NumberPositive]
        public int SuccessCount { get; set; }

        [NumberPositive]
        public int FailedCount { get; set; }

        [NotDefaultValue]
        public DateTime StartedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        [MessageMaxLength(1000)]
        public string? FailureReason { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<OutstandingDeductionTarget> Targets { get; set; } = [];
    }
}
