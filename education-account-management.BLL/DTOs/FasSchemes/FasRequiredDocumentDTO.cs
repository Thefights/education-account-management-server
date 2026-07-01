namespace DTOs.FasSchemes
{
    public sealed class FasRequiredDocumentDTO
    {
        public int Id { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public string? TemplateFileKey { get; set; }
        public int DisplayOrder { get; set; }
    }

    public sealed class FasRequiredDocumentRequestDTO
    {
        [MessageRequired, MessageMaxLength(150)]
        public string DocumentName { get; set; } = string.Empty;

        [MessageMaxLength(500)]
        public string? TemplateFileKey { get; set; }

        public IFormFile? TemplateFile { get; set; }

        public int DisplayOrder { get; set; }
    }
}
