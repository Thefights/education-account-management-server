namespace Models
{
    public class PaymentAllocation : AuditEntity
    {
        // Payment chứa allocation này.
        [NotDefaultValue]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;

        // Charge được payment allocate vào.
        [NotDefaultValue]
        public int ChargeId { get; set; }
        public Charge Charge { get; set; } = null!;

        // Kỳ trả góp cụ thể mà payment này trả vào; null nếu payment allocate trực tiếp vào charge.
        public int? ChargeInstallmentId { get; set; }
        public ChargeInstallment? ChargeInstallment { get; set; }

        // Tên course snapshot tại thời điểm allocate payment.
        [MessageRequired, MessageMaxLength(150)]
        public string CourseNameSnapshot { get; set; } = string.Empty;

        // Tên trường snapshot tại thời điểm allocate payment.
        [MessageRequired, MessageMaxLength(150)]
        public string SchoolNameSnapshot { get; set; } = string.Empty;

        // Gross amount của charge snapshot trước allocation.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal ChargeGrossAmountSnapshot { get; set; }

        // Net amount của charge snapshot trước allocation.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal ChargeNetAmountSnapshot { get; set; }

        // Remaining amount của charge snapshot trước allocation.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal ChargeRemainingAmountSnapshot { get; set; }

        // Số tiền của payment được allocate vào charge/installment.
        [Column(TypeName = "decimal(18,2)"), NumberHigherThan(0)]
        public decimal Amount { get; set; }
    }
}
