namespace DTOs.SchoolStudents
{
    public class CreateSchoolStudentDTO
    {
        [MessageRequired, SingaporeNric]
        public string Nric { get; set; } = string.Empty;
    }

    public class UpdateSchoolStudentDTO
    {
        public List<int> ListIds { get; set; } = [];

        [MessageRequired]
        public int Status { get; set; }

        [MessageRequired, MessageMinLength(10), MessageMaxLength(500)]
        public string Reason { get; set; } = string.Empty;
    }

    public class GetSchoolStudentDTO
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? Status { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public string Nric { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public List<GetSchoolStudentCourseDTO> Courses { get; set; } = [];
    }

    public class GetSchoolStudentCourseDTO
    {
        public int Id { get; set; }

        public string CourseCode { get; set; } = string.Empty;

        public string CourseName { get; set; } = string.Empty;

        public string? Status { get; set; }
    }
}
