namespace DTOs.Csv
{
    public class BatchImportProductRequestDTO
    {
        [AllowFileType(FileType.Document), MessageRequiredFile]
        public IFormFile File { get; set; } = null!;
    }
}
