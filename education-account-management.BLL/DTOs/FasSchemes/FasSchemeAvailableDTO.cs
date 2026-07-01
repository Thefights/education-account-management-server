namespace DTOs.FasSchemes
{
    public class FasSchemeAvailableResponseDTO
    {
        public decimal? CalculatedPerCapitaIncome { get; set; }
        public List<FasSchemeAvailableDTO> Schemes { get; set; } = [];
    }

    public class FasSchemeAvailableDTO
    {
        public int Id { get; set; }
        public string SchemeCode { get; set; } = string.Empty;
        public string SchemeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DurationInMonths { get; set; }
        public DateTime? PublishedAt { get; set; }

        public List<FasSchemeTierDTO> Tiers { get; set; } = [];
        public List<FasSchemeRequiredDocumentDTO> RequiredDocuments { get; set; } = [];
        public List<string> ConditionsSummary { get; set; } = [];
        public List<FasSchemeAdditionalQuestionDTO> AdditionalQuestions { get; set; } = [];
    }

    public class FasSchemeAdditionalQuestionDTO
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
    }

    public class FasSchemeRequiredDocumentDTO
    {
        public int Id { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public string? TemplateUrl { get; set; }
    }
}
