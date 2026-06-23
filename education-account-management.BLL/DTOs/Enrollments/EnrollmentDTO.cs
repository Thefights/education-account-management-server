namespace DTOs.Enrollments
{
    public class AssignEnrollmentsDTO
    {
        public int CourseId { get; set; }

        [MessageMinLength(1)]
        public List<int> SchoolStudentIds { get; set; } = [];
    }

    public class RemoveSelectedEnrollmentsDTO
    {
        [MessageMinLength(1)]
        public List<int> Ids { get; set; } = [];
    }

    public class GetEnrollmentDTO
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string CourseCode { get; set; } = string.Empty;

        public string? CourseStatus { get; set; }

        public int SchoolStudentId { get; set; }

        public string SchoolName { get; set; } = string.Empty;

        public string CourseName { get; set; } = string.Empty;

        public string? CourseDescription { get; set; }

        public string CitizenNric { get; set; } = string.Empty;

        public string CitizenFullName { get; set; } = string.Empty;

        public string? CitizenEmail { get; set; }

        public string? CitizenPhoneNumber { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public DateTime EnrolledAt { get; set; }

        public string? ChargeStatus { get; set; }

        public decimal? GrossAmount { get; set; }

        public decimal? PaidAmount { get; set; }

        public decimal? RemainingAmount { get; set; }
    }
}