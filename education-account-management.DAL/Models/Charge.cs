namespace Models
{
    public class Charge : AuditEntity
    {
        [NotDefaultValue, Unique]
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;

        [EnumDefined]
        public ChargeStatus Status { get; set; } = ChargeStatus.Unpaid;

        [MessageRequired, MessageMaxLength(150)]
        public string SchoolNameSnapshot { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(16)]
        public string CourseCodeSnapshot { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(150)]
        public string CourseNameSnapshot { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? CourseDescriptionSnapshot { get; set; }

        [NotDefaultValue]
        public DateTime CourseStartDateSnapshot { get; set; }

        [NotDefaultValue]
        public DateTime CourseEndDateSnapshot { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal CourseFeeAmountSnapshot { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal MiscFeeAmountSnapshot { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal GstAmountSnapshot { get; set; }

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

        public DateTime? BecameOutstandingAt { get; set; }

        public DateTime? LastAutoDeductedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<PaymentAllocation> PaymentAllocations { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<OutstandingDeductionTarget> OutstandingDeductionTargets { get; set; } = [];
    }
}
