namespace DTOs.FasSchemes
{
    public sealed class FasSchemeCourseDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
    }

    public sealed class FasSchemeCourseRequestDTO
    {
        [NotDefaultValue]
        public int CourseId { get; set; }
    }
}
