using Common;
using EntityAnnotations;
using EntityAnnotations.OnDeleteAttributes;
using Enums;

namespace Models
{
    public class EducationAccount : AuditEntity
    {
        [MessageRequired, MessageMaxLength(20), Unique]
        public string AccountNumber { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal EducationCreditBalance { get; set; }

        [EnumDefined]
        public EducationAccountStatus Status { get; set; } = EducationAccountStatus.Active;

        public DateTime OpenedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        [NotDefaultValue]
        public int CitizenId { get; set; }
        public Citizen Citizen { get; set; } = null!;

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<TopupBatchTarget> TopupBatchTargets { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<AdhocTopupBatchTarget> AdhocTopupBatchTargets { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<EducationCreditTransaction> EducationCreditTransactions { get; set; } = [];
    }
}
