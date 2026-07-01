namespace Models
{
    public class ChargeInstallment : AuditEntity
    {
        [NotDefaultValue]
        public int ChargeId { get; set; }
        public Charge Charge { get; set; } = null!;

        [NumberHigherThan(0)]
        public int InstallmentNumber { get; set; }

        [EnumDefined]
        public ChargeInstallmentStatus Status { get; set; } = ChargeInstallmentStatus.PendingPayment;

        [NotDefaultValue]
        public DateTime DueDate { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberHigherThan(0)]
        public decimal Amount { get; set; }

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<PaymentAllocation> PaymentAllocations { get; set; } = [];
    }
}
