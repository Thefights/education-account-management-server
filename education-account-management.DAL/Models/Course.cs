namespace Models
{
    public class Course : AuditEntity
    {
        [NotDefaultValue]
        public int SchoolId { get; set; }
        public School School { get; set; } = null!;

        [EnumDefined]
        public CourseStatus Status { get; set; } = CourseStatus.Active;

        [MessageRequired, MessageMaxLength(150)]
        public string CourseName { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? Description { get; set; }

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<CourseFee> CourseFees { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<Enrollment> Enrollments { get; set; } = [];
    }
}
