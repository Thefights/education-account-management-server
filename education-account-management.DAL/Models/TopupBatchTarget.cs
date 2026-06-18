namespace Models
{
    public class TopupBatchTarget : AuditEntity
    {
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [EnumDefined]
        public TopupTargetStatus Status { get; set; } = TopupTargetStatus.Pending;

        [MessageMaxLength(500)]
        public string? FailureReason { get; set; }

        [NotDefaultValue]
        public int TopupBatchId { get; set; }
        public TopupBatch TopupBatch { get; set; } = null!;

        [NotDefaultValue]
        public int EducationAccountId { get; set; }
        public EducationAccount EducationAccount { get; set; } = null!;

        [OnDelete(OnDeleteBehavior.NoAction)]
        public TopupBatchTargetTransaction? TopupBatchTargetTransaction { get; set; }
    }
}
