namespace Models
{
    public class Payment : AuditEntity
    {
        [Unique]
        public int? EducationCreditTransactionId { get; set; }
        public EducationCreditTransaction? EducationCreditTransaction { get; set; }

        [EnumDefined]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.EducationBalance;

        [EnumDefined]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [MessageRequired, MessageMaxLength(30)]
        public string AccountNumberSnapshot { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(20), SingaporeNric]
        public string CitizenNricSnapshot { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(150)]
        public string CitizenFullNameSnapshot { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)"), NumberHigherThan(0)]
        public decimal TotalAmount { get; set; }

        public DateTime? PaidAt { get; set; }

        [MessageMaxLength(200)]
        public string? ExternalReference { get; set; }

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<PaymentAllocation> PaymentAllocations { get; set; } = [];

        public OutstandingDeductionTarget? OutstandingDeductionTarget { get; set; }
    }
}
