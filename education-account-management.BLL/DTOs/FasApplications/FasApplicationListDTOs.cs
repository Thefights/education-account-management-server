using System.ComponentModel.DataAnnotations;

namespace DTOs.FasApplications
{
    public class GetFasApplicationListRequestDTO
    {
        [RegularExpression("^(pending|approved|expired|rejected|)$", ErrorMessage = "Status must be empty, pending, approved, expired, or rejected")]
        public string? Status { get; set; } = string.Empty; 
        
        public string? Search { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; } = 1;
        
        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
        public int PageSize { get; set; } = 10;
        
        public string? Sort { get; set; }
    }

    public class FasApplicationQueueResponseDTO
    {
        public List<FasApplicationListItemDTO> Collection { get; set; } = [];
        public FasApplicationCountsDTO Counts { get; set; } = new();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }

    public class FasApplicationListItemDTO
    {
        public string Id { get; set; } = string.Empty;
        public string ApplicantName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string SchemeName { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? ValidityEndDate { get; set; }
    }

    public class FasApplicationCountsDTO
    {
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Expired { get; set; }
        public int Rejected { get; set; }
    }
}
