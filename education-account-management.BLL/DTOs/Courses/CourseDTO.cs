namespace DTOs.Courses
{
    public class CreateCourseDTO
    {
        public string CourseName { get; set; } = string.Empty;

        public decimal CourseFeeAmount { get; set; }

        public decimal MiscFeeAmount { get; set; }

        public DateTime EnrollmentDeadline { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<int> SchoolStudentIds { get; set; } = [];

        public List<int> FasSchemeIds { get; set; } = [];
    }

    public class UpdateCourseDTO
    {
        public string CourseName { get; set; } = string.Empty;

        public decimal CourseFeeAmount { get; set; }

        public decimal MiscFeeAmount { get; set; }

        public DateTime EnrollmentDeadline { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public byte[] RowVersion { get; set; } = [];

        public List<int> FasSchemeIds { get; set; } = [];
    }

    public class GetCourseDTO
    {
        public int Id { get; set; }

        public int SchoolId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string SchoolName { get; set; } = string.Empty;

        public string? Status { get; set; }

        public string CourseCode { get; set; } = string.Empty;

        public string CourseName { get; set; } = string.Empty;

        public decimal CourseFeeAmount { get; set; }

        public decimal MiscFeeAmount { get; set; }

        public decimal GstAmount { get; set; }

        public decimal TotalFeeAmount => CourseFeeAmount + MiscFeeAmount + GstAmount;

        public DateTime EnrollmentDeadline { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int EnrollmentCount { get; set; }

        public List<GetCourseFasSchemeDTO> ApplicableFasSchemes { get; set; } = [];

        public byte[] RowVersion { get; set; } = [];
    }

    public class GetCourseFasSchemeDTO
    {
        public int Id { get; set; }

        public string SchemeCode { get; set; } = string.Empty;

        public string SchemeName { get; set; } = string.Empty;

        public string? Status { get; set; }

    }

    public class StudentCourseDetailDTO
    {
        public int Id { get; set; }

        public string CourseCode { get; set; } = string.Empty;

        public string CourseName { get; set; } = string.Empty;

        public string? Status { get; set; }

        public DateTime EnrollmentDeadline { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal CourseFeeAmount { get; set; }

        public decimal MiscFeeAmount { get; set; }

        public decimal GstAmount { get; set; }

        public decimal GrossAmount { get; set; }

        public decimal FasDeductionAmount { get; set; }

        public decimal TotalToPay { get; set; }

        public string? AppliedFasSchemeName { get; set; }

        public string? AppliedFasTierName { get; set; }

        public List<GetCourseFasSchemeDTO> ApplicableFasSchemes { get; set; } = [];
    }

    public class AssignCourseStudentsDTO
    {
        [MessageMinLength(1)]
        public List<int> SchoolStudentIds { get; set; } = [];
    }

    public class PublishCourseDTO
    {
        public List<int> Ids { get; set; } = [];

        [MessageRequired, MessageMinLength(10), MessageMaxLength(500)]
        public string Reason { get; set; } = string.Empty;
    }

    public class AssignCourseFasSchemesDTO
    {
        public List<int> FasSchemeIds { get; set; } = [];
    }

    public class DeleteSelectedCoursesDTO
    {
        public List<DeleteCourseItemDTO> Items { get; set; } = [];

        [MessageRequired, MessageMinLength(10), MessageMaxLength(500)]
        public string Reason { get; set; } = string.Empty;
    }

    public class DeleteCourseItemDTO
    {
        public int Id { get; set; }

        public byte[] RowVersion { get; set; } = [];
    }
}
