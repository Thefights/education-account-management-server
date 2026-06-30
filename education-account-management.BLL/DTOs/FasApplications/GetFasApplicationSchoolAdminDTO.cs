namespace DTOs.FasApplications
{
    public class GetFasApplicationSchoolAdminDTO
    {
        public int Id { get; set; }
        public string ApplicationNumber { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string SchemeName { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class GetFasApplicationSchoolAdminDetailDTO
    {
        public int Id { get; set; }
        public string ApplicationNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public StudentProfileDTO StudentProfile { get; set; } = new();
        public SchemeDetailsDTO Scheme { get; set; } = new();
        public SystemSuggestedTierDTO? SystemSuggestedTier { get; set; }
        public ApprovedTierDTO? ApprovedTier { get; set; }
        public List<TierOverrideHistoryDTO> TierOverrideHistories { get; set; } = [];
        public List<ApplicationAdditionalAnswerDTO> AdditionalAnswers { get; set; } = [];
    }

    public class StudentProfileDTO
    {
        public int Age { get; set; }
        public string? StudentNationality { get; set; }
        public string? GuardianNationality { get; set; }
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
        public decimal? SubsidyValue { get; set; }
        public decimal? CourseFeeSubsidyValue { get; set; }
        public decimal? MiscFeeSubsidyValue { get; set; }
        public decimal? MaxPerCapitaIncome { get; set; }
        public decimal? MaxGrossHouseholdIncome { get; set; }
    }

    public class ApplicationDocumentDTO
    {
        public int RequiredDocumentId { get; set; }
        public int? ApplicationDocumentId { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public string? FileName { get; set; }
        public string? FileKey { get; set; }
    }

    public class SystemSuggestedTierDTO
    {
        public int Id { get; set; }
        public string TierName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }

    public class ApprovedTierDTO
    {
        public int Id { get; set; }
        public string TierName { get; set; } = string.Empty;
    }

    public class TierOverrideHistoryDTO
    {
        public int Id { get; set; }
        public int? OldTierId { get; set; }
        public string? OldTierName { get; set; }
        public int NewTierId { get; set; }
        public string NewTierName { get; set; } = string.Empty;
        public string RecommendationReason { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public int ModifiedByUserId { get; set; }
        public string? ModifiedByName { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    public class ApplicationAdditionalAnswerDTO
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string? AnswerText { get; set; }
        public bool IsRequired { get; set; }
    }
}
