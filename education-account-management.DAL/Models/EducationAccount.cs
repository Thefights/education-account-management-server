using EntityAnnotations.DateAttributes;

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

        public DateTime OpenedAt { get; set; } = DateTime.UtcNow;

        [DateValidator(NotBefore = nameof(OpenedAt))]
        public DateTime? ClosedAt { get; set; }

        [NotDefaultValue]
        public int CitizenId { get; set; }
        public Citizen Citizen { get; set; } = null!;

        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        public SchoolStudent? SchoolStudent { get; set; }

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<TopupExecutionTarget> TopupExecutionTargets { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<TopupSystemApplication> TopupSystemApplications { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<EducationCreditTransaction> EducationCreditTransactions { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<OutstandingDeductionTarget> OutstandingDeductionTargets { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<EducationAccountStatusHistory> StatusHistories { get; set; } = [];
    }
}