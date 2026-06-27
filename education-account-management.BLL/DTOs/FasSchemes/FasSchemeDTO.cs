namespace DTOs.FasSchemes
{
    public sealed class GetFasSchemeDTO
    {
        public int Id { get; set; }
        public int SchoolId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Status { get; set; }
        public string SchemeCode { get; set; } = string.Empty;
        public string SchemeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DurationInMonths { get; set; }
        public string? SubsidyType { get; set; }
        public bool IsPerComponent { get; set; }
        public DateTime? PublishedAt { get; set; }
        public FasConditionGroupDTO? RootConditionGroup { get; set; }
        public List<FasSchemeTierDTO> Tiers { get; set; } = [];
        public List<FasRequiredDocumentDTO> RequiredDocuments { get; set; } = [];
        public List<FasSchemeCourseDTO> SchemeCourses { get; set; } = [];
    }

    public sealed class CreateFasSchemeDTO
    {
        [MessageRequired, MessageMaxLength(150)]
        public string SchemeName { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? Description { get; set; }

        public int DurationInMonths { get; set; }

        [EnumDefined]
        public FasSubsidyType SubsidyType { get; set; }

        public bool IsPerComponent { get; set; }

        public FasConditionGroupRequestDTO RootConditionGroup { get; set; } = new();
        public List<FasSchemeTierRequestDTO> Tiers { get; set; } = [];
        public List<FasRequiredDocumentRequestDTO> RequiredDocuments { get; set; } = [];
        public List<FasSchemeCourseRequestDTO> SchemeCourses { get; set; } = [];
    }

    public sealed class UpdateFasSchemeDTO
    {
        [MessageRequired, MessageMaxLength(150)]
        public string SchemeName { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? Description { get; set; }

        public int DurationInMonths { get; set; }

        [EnumDefined]
        public FasSubsidyType SubsidyType { get; set; }

        public bool IsPerComponent { get; set; }

        public FasConditionGroupRequestDTO RootConditionGroup { get; set; } = new();
        public List<FasSchemeTierRequestDTO> Tiers { get; set; } = [];
        public List<FasRequiredDocumentRequestDTO> RequiredDocuments { get; set; } = [];
        public List<FasSchemeCourseRequestDTO> SchemeCourses { get; set; } = [];
    }

    public sealed class BatchUpdateFasSchemeStatusDTO
    {
        [MessageMinLength(1)]
        public List<int> Ids { get; set; } = [];

        [EnumDefined]
        public FasSchemeStatus Status { get; set; }
    }
}
