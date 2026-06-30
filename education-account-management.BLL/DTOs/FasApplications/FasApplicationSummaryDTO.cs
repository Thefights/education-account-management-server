using Enums;

namespace DTOs.FasApplications
{
    public class FasApplicationSummaryDTO
    {
        public int Id { get; set; }
        public string ApplicationNumber { get; set; } = string.Empty;
        public int SchemeId { get; set; }
        public string SchemeName { get; set; } = string.Empty;
        public FasApplicationStatus Status { get; set; }
        public DateTimeOffset SubmittedAt { get; set; }
        public DateTimeOffset? ApprovedDate { get; set; }
        public DateTimeOffset? ValidityEndDate { get; set; }
        public string? ExternalRejectionReason { get; set; }
    }
}
