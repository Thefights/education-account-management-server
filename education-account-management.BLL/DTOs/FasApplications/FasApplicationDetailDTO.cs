using Enums;

namespace DTOs.FasApplications
{
    public class FasApplicationDocumentDetailDTO
    {
        public int Id { get; set; }
        public int? RequiredDocumentId { get; set; }
        public string DocumentNameSnapshot { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileKey { get; set; } = string.Empty;
    }

    public class FasSchemeBasicInfoDTO
    {
        public int Id { get; set; }
        public string SchemeCode { get; set; } = string.Empty;
        public string SchemeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class FasApplicationTierDetailDTO
    {
        public string TierName { get; set; } = string.Empty;
        public decimal? SubsidyValue { get; set; }
        public decimal? CourseFeeSubsidyValue { get; set; }
        public decimal? MiscFeeSubsidyValue { get; set; }
    }

    public class FasApplicationDetailDTO
    {
        public int Id { get; set; }
        public string ApplicationNumber { get; set; } = string.Empty;
        public FasApplicationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? WithdrawnAt { get; set; }

        public FasSchemeBasicInfoDTO Scheme { get; set; } = null!;

        public int StudentAgeSnapshot { get; set; }
        public NationalityCategory StudentNationalitySnapshot { get; set; }
        public NationalityCategory GuardianNationalitySnapshot { get; set; }
        
        public decimal GrossHouseholdIncomeSnapshot { get; set; }
        public int HouseholdMemberCountSnapshot { get; set; }
        public decimal PerCapitaIncomeSnapshot { get; set; }

        public string? RejectionReason { get; set; }

        public DateTime? ApprovedAt { get; set; }
        public DateTime? ValidityStartDate { get; set; }
        public DateTime? ValidityEndDate { get; set; }
        public FasApplicationTierDetailDTO? ApprovedTier { get; set; }

        public List<FasApplicationDocumentDetailDTO> Documents { get; set; } = [];
        public List<FasApplicationAdditionalAnswerDetailDTO> AdditionalAnswers { get; set; } = [];
    }

    public class FasApplicationAdditionalAnswerDetailDTO
    {
        public int Id { get; set; }
        public int? FasSchemeAdditionalQuestionId { get; set; }
        public string QuestionTextSnapshot { get; set; } = string.Empty;
        public bool IsRequiredSnapshot { get; set; }
        public string? AnswerText { get; set; }
    }
}
