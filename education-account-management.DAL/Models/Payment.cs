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

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal TotalAmount { get; set; }

        public DateTime? PaidAt { get; set; }

        [MessageMaxLength(256)]
        public string? ExternalReference { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<PaymentAllocation> PaymentAllocations { get; set; } = [];
    }
}
