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
        public decimal GrossAmount { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive, NumberLessThanOrEqualTo(nameof(GrossAmount))]
        public decimal SubsidyAmount { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal NetAmount { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive, NumberLessThanOrEqualTo(nameof(NetAmount))]
        public decimal PaidAmount { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive, NumberLessThanOrEqualTo(nameof(NetAmount))]
        public decimal RemainingAmount { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<PaymentAllocation> PaymentAllocations { get; set; } = [];
    }
}