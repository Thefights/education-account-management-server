

namespace DTOs.FasApplications
{
    public class FasApplicationItemDTO
    {
        public string Id { get; set; } = string.Empty;
        public string ApplicantNo { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string AccountNo { get; set; } = string.Empty;
        public string SchemeName { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public string Status { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? ValidityEndDate { get; set; }
    }

     public class FasApplicationDetailDTO
    {
        public string ApplicationId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public StudentProfileDTO StudentProfile { get; set; } = new();
        public SchemeDetailsDTO Scheme { get; set; } = new();
        public SystemSuggestedTierDTO? SystemSuggestedTier { get; set; }
    }

    public class StudentProfileDTO
    {
        public int Age { get; set; }
        public NationalityCategory StudentNationality { get; set; } = NationalityCategory.SingaporeCitizen;
        public NationalityCategory GuardianNationality { get; set; } = NationalityCategory.SingaporeCitizen;
        public decimal GrossHouseholdIncome { get; set; }
        public int HouseholdMembers { get; set; }
        public decimal PerCapitaIncome { get; set; }
    }

    public class SchemeDetailsDTO
    {
        public int Id { get; set; }
        public string SchemeName { get; set; } = string.Empty;
        public List<TierDetailsDTO> Tiers { get; set; } = [];
        public List<ApplicationDocumentDTO> RequiredDocuments { get; set; } = [];
    }

    public class TierDetailsDTO
    {
        public int Id { get; set; }
        public string TierName { get; set; } = string.Empty;
        public string ConditionText { get; set; } = string.Empty;
        public string SubsidyDescription { get; set; } = string.Empty;
        public decimal? MaxPerCapitaIncome { get; set; }
    }

    public class ApplicationDocumentDTO
    {
        public int Id { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? FileUrl { get; set; }
    }

    public class SystemSuggestedTierDTO
    {
        public int Id { get; set; }
        public string TierName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}
