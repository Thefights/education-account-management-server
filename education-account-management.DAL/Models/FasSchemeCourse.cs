namespace Models
{
    public class FasSchemeCourse : AuditEntity
    {
        // Scheme được link với course.
        [NotDefaultValue]
        public int FasSchemeId { get; set; }
        public FasScheme FasScheme { get; set; } = null!;

        // Course được phép áp dụng scheme.
        [NotDefaultValue]
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}
