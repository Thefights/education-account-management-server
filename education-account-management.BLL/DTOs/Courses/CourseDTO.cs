namespace DTOs.Courses
{
    public class CreateCourseDTO
    {
        public string CourseName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal CourseFeeAmount { get; set; }

        public decimal MiscFeeAmount { get; set; }

        public DateTime FasApplicationDueDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class UpdateCourseDTO
    {
        public string CourseName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal CourseFeeAmount { get; set; }

        public decimal MiscFeeAmount { get; set; }

        public DateTime FasApplicationDueDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public byte[] RowVersion { get; set; } = [];
    }

    public class GetCourseDTO
    {
        public int Id { get; set; }

        public int SchoolId { get; set; }

        public string SchoolName { get; set; } = string.Empty;

        public string? Status { get; set; }

        public string CourseCode { get; set; } = string.Empty;

        public string CourseName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal CourseFeeAmount { get; set; }

        public decimal MiscFeeAmount { get; set; }

        public decimal GstAmount { get; set; }

        public decimal TotalFeeAmount => CourseFeeAmount + MiscFeeAmount + GstAmount;

        public DateTime FasApplicationDueDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int EnrollmentCount { get; set; }

        public byte[] RowVersion { get; set; } = [];
    }

    public class AssignCourseStudentsDTO
    {
        [MessageMinLength(1)]
        public List<int> SchoolStudentIds { get; set; } = [];
    }

    public class PublishCourseDTO
    {
        public List<int> Ids { get; set; } = [];
    }

    public class DeleteSelectedCoursesDTO
    {
        public List<DeleteCourseItemDTO> Items { get; set; } = [];
    }

    public class DeleteCourseItemDTO
    {
        public int Id { get; set; }

        public byte[] RowVersion { get; set; } = [];
    }
}

