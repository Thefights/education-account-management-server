using System.ComponentModel.DataAnnotations;

namespace DTOs.FasApplications
{

    public class GetFasApplicationSchoolAdminDTO
    {
        public int Id { get; set; }
        public string ApplicationNumber { get; set; } = string.Empty;
        public string AccountName {  get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string SchemeName { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public Enums.FasApplicationStatus Status { get; set; }

    }

    public class GetFasApplicationSchoolAdminDetailDTO
    {
        public int id { get; set; }
        public string ApplicationNumber { get; set; } = string.Empty;
        public Enums.FasApplicationStatus Status { get; set; }
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
