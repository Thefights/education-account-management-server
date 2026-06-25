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

        public string? SubsidyType { get; set; }

        public bool IsPerComponent { get; set; }

        public int DurationInMonths { get; set; }

        public List<GetCourseFasSchemeTierDTO> Tiers { get; set; } = [];
    }

    public class GetCourseFasSchemeTierDTO
    {
        public int Id { get; set; }

        public string TierName { get; set; } = string.Empty;

        public decimal? MaxPerCapitaIncome { get; set; }

        public decimal? SubsidyValue { get; set; }

        public decimal? CourseFeeSubsidyValue { get; set; }

        public decimal? MiscFeeSubsidyValue { get; set; }

        public int DisplayOrder { get; set; }
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

    public class AssignCourseFasSchemesDTO
    {
        public List<int> FasSchemeIds { get; set; } = [];
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
