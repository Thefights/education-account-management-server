using Common;

namespace Models
{
    public class AdhocTopupBatchTargetTransaction : AuditEntity
    {
        [NotDefaultValue]
        public int AdhocTopupBatchTargetId { get; set; }
        public AdhocTopupBatchTarget AdhocTopupBatchTarget { get; set; } = null!;

        [NotDefaultValue]
        public int EducationCreditTransactionId { get; set; }
        public EducationCreditTransaction EducationCreditTransaction { get; set; } = null!;
    }
}
