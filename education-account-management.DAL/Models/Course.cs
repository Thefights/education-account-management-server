using EntityAnnotations.DateAttributes;

namespace Models
{
    public class Course : AuditEntity
    {
        [NotDefaultValue]
        public int SchoolId { get; set; }
        public School School { get; set; } = null!;

        [EnumDefined]
        public CourseStatus Status { get; set; } = CourseStatus.Draft;

        [MessageRequired, MessageMaxLength(16), RegularExpression(@"^CRS-\d{4}-[A-Z0-9]{7}$"), Unique]
        public string CourseCode { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(150), Unique]
        public string CourseName { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal CourseFeeAmount { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal MiscFeeAmount { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal GstAmount { get; set; }

        [NotDefaultValue, DateValidator(NotAfter = nameof(StartDate))]
        public DateTime FasApplicationDueDate { get; set; }

        [NotDefaultValue, DateValidator(NotBefore = nameof(FasApplicationDueDate), NotAfter = nameof(EndDate))]
        public DateTime StartDate { get; set; }

        [NotDefaultValue, DateValidator(NotBefore = nameof(StartDate))]
        public DateTime EndDate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<Enrollment> Enrollments { get; set; } = [];
    }
}
