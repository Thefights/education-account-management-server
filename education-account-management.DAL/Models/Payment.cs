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

        [MessageRequired, MessageMaxLength(20)]
        public string AccountNumberSnapshot { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(9)]
        public string CitizenNricSnapshot { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(150)]
        public string CitizenFullNameSnapshot { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal TotalAmount { get; set; }

        public DateTime? PaidAt { get; set; }

        [MessageMaxLength(256)]
        public string? ExternalReference { get; set; }

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<PaymentAllocation> PaymentAllocations { get; set; } = [];
    }
}