using EntityAnnotations.DateAttributes;

namespace Models
{
    public class Course : AuditEntity
    {
        // Trường sở hữu course này.
        [NotDefaultValue]
        public int SchoolId { get; set; }
        public School School { get; set; } = null!;

        // Trạng thái lifecycle của course.
        [EnumDefined]
        public CourseStatus Status { get; set; } = CourseStatus.Draft;

        // Mã course tự sinh để admin/student tra cứu.
        [MessageRequired, MessageMaxLength(16), RegularExpression(@"^CRS-\d{4}-[A-Z0-9]{7}$"), Unique]
        public string CourseCode { get; set; } = string.Empty;

        // Tên course hiển thị trên admin/payment.
        [MessageRequired, MessageMaxLength(150), Unique]
        public string CourseName { get; set; } = string.Empty;

        // Mô tả nội dung course.
        [MessageMaxLength(1000)]
        public string? Description { get; set; }

        // Phí course gốc trước tax và FAS deduction.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal CourseFeeAmount { get; set; }

        // Phí misc gốc trước tax và FAS deduction.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal MiscFeeAmount { get; set; }

        // Legacy: GST amount cũ đang được logic hiện tại sử dụng; charge mới nên snapshot tax từ setting.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal GstAmount { get; set; }

        // Deadline để finalize enrollment và chuyển course từ Enrolling sang Upcoming.
        [NotDefaultValue, DateValidator(NotAfter = nameof(StartDate))]
        public DateTime EnrollmentDeadline { get; set; }

        // Legacy: deadline cũ từng dùng cho FAS/course payment; DEV sẽ thay dần bằng EnrollmentDeadline.
        [NotDefaultValue, DateValidator(NotAfter = nameof(StartDate))]
        public DateTime FasApplicationDueDate { get; set; }

        // Cho phép student thanh toán toàn bộ charge một lần.
        public bool AllowFullPayment { get; set; } = true;

        // Cho phép student chọn trả góp 3 tháng.
        public bool AllowInstallment3Months { get; set; }

        // Cho phép student chọn trả góp 6 tháng.
        public bool AllowInstallment6Months { get; set; }

        // Cho phép student chọn trả góp 12 tháng.
        public bool AllowInstallment12Months { get; set; }

        // Ngày bắt đầu course.
        [NotDefaultValue, DateValidator(NotBefore = nameof(FasApplicationDueDate), NotAfter = nameof(EndDate))]
        public DateTime StartDate { get; set; }

        // Ngày kết thúc course.
        [NotDefaultValue, DateValidator(NotBefore = nameof(StartDate))]
        public DateTime EndDate { get; set; }

        // Token concurrency để chống update đè khi nhiều admin chỉnh course.
        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        // Danh sách enrollment của course.
        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<Enrollment> Enrollments { get; set; } = [];

        // Các FAS scheme được phép apply vào course này.
        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<FasSchemeCourse> FasSchemeCourses { get; set; } = [];
    }
}
