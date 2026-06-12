namespace DTOs.Auth
{
    public class BatchImportAuthAccountRequestDTO
    {
        [AllowFileType(FileType.Document), MessageRequiredFile]
        public IFormFile File { get; set; } = null!;

        public bool SendEmail { get; set; } = true;
    }

    public class ImportAuthAccountCsvRowDTO
    {
        public string? UserIdText { get; set; }

        public string? Email { get; set; }

        public string? FullName { get; set; }

        public string? Gender { get; set; }

        public string? RoleIds { get; set; }

        public string? ProductAssignments { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ImageUrl { get; set; }
    }
}
