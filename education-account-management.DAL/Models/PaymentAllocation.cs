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

        [MessageRequired, MessageMaxLength(150)]
        public string CourseNameSnapshot { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(150)]
        public string SchoolNameSnapshot { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal ChargeGrossAmountSnapshot { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal ChargeNetAmountSnapshot { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal ChargeRemainingAmountSnapshot { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal Amount { get; set; }
    }
}