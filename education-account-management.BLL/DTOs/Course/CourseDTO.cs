namespace DTOs.Course
{
    public class CreateCourseDTO
    {
        public int SchoolId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal CourseFeeAmount { get; set; }

        public decimal MiscFeeAmount { get; set; }

        public decimal GstAmount { get; set; }
    }

    public class UpdateCourseDTO
    {
        public int SchoolId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal CourseFeeAmount { get; set; }

        public decimal MiscFeeAmount { get; set; }

        public decimal GstAmount { get; set; }
    }

    public class GetCourseDTO
    {
        public int Id { get; set; }

        public int SchoolId { get; set; }

        public string SchoolName { get; set; } = string.Empty;

        public string? Status { get; set; }

        public string CourseName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal CourseFeeAmount { get; set; }

        public decimal MiscFeeAmount { get; set; }

        public decimal GstAmount { get; set; }
    }
}
