namespace Models
{
    public class EducationCreditTransaction : AuditEntity
    {
        [Unique]
        public Guid TransactionCode { get; set; } = Guid.NewGuid();

        [EnumDefined]
        public EducationCreditTransactionType Type { get; set; } = EducationCreditTransactionType.Topup;

        [EnumDefined]
        public EducationCreditTransactionDirection Direction { get; set; } = EducationCreditTransactionDirection.Credit;

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceBefore { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceAfter { get; set; }

        [MessageMaxLength(500)]
        public string? Description { get; set; }

        [NotDefaultValue]
        public int EducationAccountId { get; set; }
        public EducationAccount EducationAccount { get; set; } = null!;

        [OnDelete(OnDeleteBehavior.Restrict)]
        public TopupBatchTargetTransaction? TopupBatchTargetTransaction { get; set; }

        [OnDelete(OnDeleteBehavior.Restrict)]
        public AdhocTopupBatchTargetTransaction? AdhocTopupBatchTargetTransaction { get; set; }

        [OnDelete(OnDeleteBehavior.Restrict)]
        public Payment? Payment { get; set; }
    }
}
