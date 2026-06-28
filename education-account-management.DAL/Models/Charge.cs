namespace Models
{
    public class Charge : AuditEntity
    {
        [EnumDefined]
        public ChargeStatus Status { get; set; } = ChargeStatus.PendingPayment;

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal GrossAmount { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive, NumberLessThanOrEqualTo(nameof(GrossAmount))]
        public decimal SubsidyAmount { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal NetAmount { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive, NumberLessThanOrEqualTo(nameof(NetAmount))]
        public decimal PaidAmount { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive, NumberLessThanOrEqualTo(nameof(NetAmount))]
        public decimal RemainingAmount { get; set; }

        [PaymentPlanMonths]
        public int? PaymentPlanMonths { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        public string SchoolNameSnapshot { get; set; } = string.Empty;

        public string CourseCodeSnapshot { get; set; } = string.Empty;

        public string CourseNameSnapshot { get; set; } = string.Empty;

        public string? CourseDescriptionSnapshot { get; set; }

        public DateTime CourseStartDateSnapshot { get; set; }

        public DateTime CourseEndDateSnapshot { get; set; }

        public decimal CourseFeeAmountSnapshot { get; set; }

        public decimal MiscFeeAmountSnapshot { get; set; }

        public decimal GstAmountSnapshot { get; set; }

        public decimal TaxRateSnapshot { get; set; }

        public string? AppliedFasSchemeNameSnapshot { get; set; }

        public string? AppliedFasTierNameSnapshot { get; set; }

        public FasSubsidyType? AppliedFasSubsidyTypeSnapshot { get; set; }

        public bool AppliedFasIsPerComponentSnapshot { get; set; }

        public decimal? AppliedFasSubsidyValueSnapshot { get; set; }

        public decimal? AppliedFasCourseFeeSubsidyValueSnapshot { get; set; }

        public decimal? AppliedFasMiscFeeSubsidyValueSnapshot { get; set; }

        [NotDefaultValue, Unique]
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;

        public int? AppliedFasApplicationId { get; set; }
        public FasApplication? AppliedFasApplication { get; set; }

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<PaymentAllocation> PaymentAllocations { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<ChargeInstallment> Installments { get; set; } = [];
    }
}
