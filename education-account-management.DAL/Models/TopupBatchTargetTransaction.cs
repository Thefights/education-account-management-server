using Common;

namespace Models
{
    public class TopupBatchTargetTransaction : AuditEntity
    {
        [NotDefaultValue]
        public int TopupBatchTargetId { get; set; }
        public TopupBatchTarget TopupBatchTarget { get; set; } = null!;

        [NotDefaultValue]
        public int EducationCreditTransactionId { get; set; }
        public EducationCreditTransaction EducationCreditTransaction { get; set; } = null!;
    }
}
