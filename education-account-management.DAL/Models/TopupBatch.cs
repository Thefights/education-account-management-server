using Common;
using EntityAnnotations.OnDeleteAttributes;
using Enums;

namespace Models
{
    public class TopupBatch : AuditEntity
    {
        [MessageRequired, MessageMaxLength(50), Unique]
        public string BatchCode { get; set; } = string.Empty;

        [EnumDefined]
        public TopupBatchStatus Status { get; set; } = TopupBatchStatus.Executing;

        [NumberPositive]
        public int TotalTargetCount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime? ExecutedAt { get; set; }

        [NotDefaultValue]
        public int TopupRuleId { get; set; }
        public TopupRule TopupRule { get; set; } = null!;

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<TopupBatchTarget> Targets { get; set; } = [];
    }
}
