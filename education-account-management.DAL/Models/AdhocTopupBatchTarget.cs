namespace Models
{
    public class AdhocTopupBatchTarget : AuditEntity
    {
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [EnumDefined]
        public TopupTargetStatus Status { get; set; } = TopupTargetStatus.Pending;

        [MessageMaxLength(500)]
        public string? FailureReason { get; set; }

        [NotDefaultValue]
        public int AdhocTopupBatchId { get; set; }
        public AdhocTopupBatch AdhocTopupBatch { get; set; } = null!;

        [NotDefaultValue]
        public int EducationAccountId { get; set; }
        public EducationAccount EducationAccount { get; set; } = null!;

        [OnDelete(OnDeleteBehavior.NoAction)]
        public AdhocTopupBatchTargetTransaction? AdhocTopupBatchTargetTransaction { get; set; }
    }
}
