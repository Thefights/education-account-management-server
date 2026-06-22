namespace Models
{
    public class Charge : AuditEntity
    {
        [NotDefaultValue, Unique]
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;

        [EnumDefined]
        public ChargeStatus Status { get; set; } = ChargeStatus.Unpaid;

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

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<PaymentAllocation> PaymentAllocations { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<OutstandingDeductionTarget> OutstandingDeductionTargets { get; set; } = [];
    }
}
