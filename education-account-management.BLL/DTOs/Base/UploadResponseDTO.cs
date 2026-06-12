namespace DTOs.Base
{
    public class UploadResultDTO
    {
        public string FileName { get; set; } = string.Empty;

        public string PublicUrl { get; set; } = string.Empty;

        public long FileSizeBytes { get; set; }
    }
}
