namespace Models
{
    public class OutstandingDeductionTarget : AuditEntity
    {
        [NotDefaultValue]
        public int OutstandingDeductionRunId { get; set; }
        public OutstandingDeductionRun OutstandingDeductionRun { get; set; } = null!;

        [NotDefaultValue]
        public int ChargeId { get; set; }
        public Charge Charge { get; set; } = null!;

        [NotDefaultValue]
        public int EducationAccountId { get; set; }
        public EducationAccount EducationAccount { get; set; } = null!;

        [Unique]
        public int? EducationCreditTransactionId { get; set; }
        public EducationCreditTransaction? EducationCreditTransaction { get; set; }

        [Unique]
        public int? PaymentId { get; set; }
        public Payment? Payment { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal BalanceBefore { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal RemainingBefore { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal DeductedAmount { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal BalanceAfter { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal RemainingAfter { get; set; }

        [EnumDefined]
        public OutstandingDeductionTargetStatus Status { get; set; }

        [MessageMaxLength(1000)]
        public string? FailureReason { get; set; }
    }
}
