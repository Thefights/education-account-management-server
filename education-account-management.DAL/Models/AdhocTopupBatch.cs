namespace Models
{
    public class AdhocTopupBatch : AuditEntity
    {
        [MessageRequired, MessageMaxLength(500)]
        public string Reason { get; set; } = string.Empty;

        [EnumDefined]
        public TopupBatchStatus Status { get; set; } = TopupBatchStatus.Executing;

        [NumberPositive]
        public int TotalTargetCount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime? ExecutedAt { get; set; }

        [OnDelete(OnDeleteBehavior.NoAction)]
        public ICollection<AdhocTopupBatchTarget> Targets { get; set; } = [];
    }
}
