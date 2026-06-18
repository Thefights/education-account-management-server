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

        public DateTime OpenedAt { get; set; }

        [DateValidator(NotBefore = nameof(OpenedAt))]
        public DateTime? ClosedAt { get; set; }

        [DateValidator(NotBefore = nameof(OpenedAt))]
        public DateTime? ExtendedUntil { get; set; }

        public int? OpenedByUserId { get; set; }
        [ForeignKey(nameof(OpenedByUserId))]
        public User? OpenedByUser { get; set; }

        public int? ClosedByUserId { get; set; }
        [ForeignKey(nameof(ClosedByUserId))]
        public User? ClosedByUser { get; set; }

        [NotDefaultValue, Unique]
        public int CitizenId { get; set; }
        public Citizen Citizen { get; set; } = null!;

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<TopupBatchTarget> TopupBatchTargets { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<AdhocTopupBatchTarget> AdhocTopupBatchTargets { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<EducationCreditTransaction> EducationCreditTransactions { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<Enrollment> Enrollments { get; set; } = [];

    }
}
