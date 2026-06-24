namespace Models
{
    public class Charge : AuditEntity
    {
        // Enrollment mà charge/invoice này được generate cho.
        [NotDefaultValue, Unique]
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;

        // Approved FAS application được system chọn là best choice cho charge này.
        public int? AppliedFasApplicationId { get; set; }
        public FasApplication? AppliedFasApplication { get; set; }

        // Trạng thái thanh toán/outstanding của charge.
        [EnumDefined]
        public ChargeStatus Status { get; set; } = ChargeStatus.Unpaid;

        // Tên trường snapshot tại thời điểm generate charge.
        [MessageRequired, MessageMaxLength(150)]
        public string SchoolNameSnapshot { get; set; } = string.Empty;

        // Mã course snapshot tại thời điểm generate charge.
        [MessageRequired, MessageMaxLength(16)]
        public string CourseCodeSnapshot { get; set; } = string.Empty;

        // Tên course snapshot tại thời điểm generate charge.
        [MessageRequired, MessageMaxLength(150)]
        public string CourseNameSnapshot { get; set; } = string.Empty;

        // Mô tả course snapshot tại thời điểm generate charge.
        [MessageMaxLength(1000)]
        public string? CourseDescriptionSnapshot { get; set; }

        // Ngày bắt đầu course snapshot tại thời điểm generate charge.
        [NotDefaultValue]
        public DateTime CourseStartDateSnapshot { get; set; }

        // Ngày kết thúc course snapshot tại thời điểm generate charge.
        [NotDefaultValue]
        public DateTime CourseEndDateSnapshot { get; set; }

        // Course fee snapshot tại thời điểm generate charge.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal CourseFeeAmountSnapshot { get; set; }

        // Misc fee snapshot tại thời điểm generate charge.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal MiscFeeAmountSnapshot { get; set; }

        // Tax/GST amount snapshot tại thời điểm generate charge.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal GstAmountSnapshot { get; set; }

        // Tax rate lấy từ setting chung tại thời điểm generate charge.
        [Column(TypeName = "decimal(5,4)"), NumberPositive]
        public decimal TaxRateSnapshot { get; set; }

        // Tên scheme FAS được apply, snapshot để lịch sử không đổi khi scheme đổi tên.
        [MessageMaxLength(150)]
        public string? AppliedFasSchemeNameSnapshot { get; set; }

        // Tên tier FAS được apply, snapshot để lịch sử không đổi khi tier đổi tên.
        [MessageMaxLength(100)]
        public string? AppliedFasTierNameSnapshot { get; set; }

        // Loại subsidy của FAS được apply, snapshot để audit cách tính deduction.
        public FasSubsidyType? AppliedFasSubsidyTypeSnapshot { get; set; }

        // Snapshot cho biết FAS được apply có tách Course Fee/Misc Fee hay không.
        public bool AppliedFasIsPerComponentSnapshot { get; set; }

        // Giá trị subsidy chính của FAS được apply khi không tách Course Fee/Misc Fee.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal? AppliedFasSubsidyValueSnapshot { get; set; }

        // Giá trị hỗ trợ Course Fee khi FAS dùng per-component.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal? AppliedFasCourseFeeSubsidyValueSnapshot { get; set; }

        // Giá trị hỗ trợ Misc Fee khi FAS dùng per-component.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal? AppliedFasMiscFeeSubsidyValueSnapshot { get; set; }

        // Tổng tiền trước FAS deduction.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal GrossAmount { get; set; }

        // Số tiền FAS deduction được áp dụng cho charge.
        [Column(TypeName = "decimal(18,2)"), NumberPositive, NumberLessThanOrEqualTo(nameof(GrossAmount))]
        public decimal SubsidyAmount { get; set; }

        // Số tiền student cần trả sau FAS deduction.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal NetAmount { get; set; }

        // Tổng số tiền đã thanh toán vào charge.
        [Column(TypeName = "decimal(18,2)"), NumberPositive, NumberLessThanOrEqualTo(nameof(NetAmount))]
        public decimal PaidAmount { get; set; }

        // Số tiền còn lại cần thanh toán.
        [Column(TypeName = "decimal(18,2)"), NumberPositive, NumberLessThanOrEqualTo(nameof(NetAmount))]
        public decimal RemainingAmount { get; set; }

        // Thời điểm charge bắt đầu được xem là outstanding.
        public DateTime? BecameOutstandingAt { get; set; }

        // Legacy: thời điểm auto-deduct cũ chạy gần nhất; requirement mới không còn dùng auto-deduct.
        public DateTime? LastAutoDeductedAt { get; set; }

        // Token concurrency để chống update payment/charge bị đè.
        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        // Các payment allocation đã thanh toán vào charge.
        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<PaymentAllocation> PaymentAllocations { get; set; } = [];

        // Danh sách kỳ trả góp của charge nếu student chọn installment.
        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<ChargeInstallment> Installments { get; set; } = [];

        // Legacy: các target auto-deduct cũ liên quan charge này.
        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<OutstandingDeductionTarget> OutstandingDeductionTargets { get; set; } = [];
    }
}
