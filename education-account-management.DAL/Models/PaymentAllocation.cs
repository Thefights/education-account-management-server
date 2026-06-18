namespace Models
{
    public class PaymentAllocation : AuditEntity
    {
        [NotDefaultValue]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;

        [NotDefaultValue]
        public int ChargeId { get; set; }
        public Charge Charge { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal Amount { get; set; }
    }
}
