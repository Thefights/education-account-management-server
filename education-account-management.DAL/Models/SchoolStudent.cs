namespace Models
{
    public class SchoolStudent : AuditEntity
    {
        [NotDefaultValue]
        public int SchoolId { get; set; }
        public School School { get; set; } = null!;

        [NotDefaultValue, Unique]
        public int EducationAccountId { get; set; }
        public EducationAccount EducationAccount { get; set; } = null!;

        [EnumDefined]
        public SchoolStudentStatus Status { get; set; } = SchoolStudentStatus.Active;

        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<Enrollment> Enrollments { get; set; } = [];
    }
}