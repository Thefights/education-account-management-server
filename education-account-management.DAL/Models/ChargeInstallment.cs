namespace Models
{
    public class ChargeInstallment : AuditEntity
    {
        // Charge/invoice chứa kỳ trả góp này.
        [NotDefaultValue]
        public int ChargeId { get; set; }
        public Charge Charge { get; set; } = null!;

        // Số thứ tự kỳ trả góp.
        [NumberHigherThan(0)]
        public int InstallmentNumber { get; set; }

        // Trạng thái thanh toán của kỳ trả góp.
        [EnumDefined]
        public ChargeInstallmentStatus Status { get; set; } = ChargeInstallmentStatus.Unpaid;

        // Ngày đến hạn thanh toán của kỳ này.
        [NotDefaultValue]
        public DateTime DueDate { get; set; }

        // Số tiền phải trả của kỳ này.
        [Column(TypeName = "decimal(18,2)"), NumberHigherThan(0)]
        public decimal Amount { get; set; }

        // Số tiền đã thanh toán cho kỳ này.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal PaidAmount { get; set; }

        // Số tiền còn lại của kỳ này.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal RemainingAmount { get; set; }

        // Thời điểm kỳ này bắt đầu bị quá hạn.
        public DateTime? BecameOverdueAt { get; set; }

        // Các allocation đã trả vào kỳ này.
        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<PaymentAllocation> PaymentAllocations { get; set; } = [];
    }
}
