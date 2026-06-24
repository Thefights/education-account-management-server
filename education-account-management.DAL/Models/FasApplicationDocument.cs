namespace Models
{
    public class FasApplicationDocument : AuditEntity
    {
        // Application sở hữu file upload này.
        [NotDefaultValue]
        public int FasApplicationId { get; set; }
        public FasApplication FasApplication { get; set; } = null!;

        // Loại giấy tờ bắt buộc từ scheme; null nếu là file bổ sung ngoài danh sách.
        public int? FasSchemeRequiredDocumentId { get; set; }
        public FasSchemeRequiredDocument? FasSchemeRequiredDocument { get; set; }

        // Tên giấy tờ snapshot để lịch sử không đổi khi scheme document đổi tên.
        [MessageRequired, MessageMaxLength(150)]
        public string DocumentNameSnapshot { get; set; } = string.Empty;

        // Storage key/path của file student upload.
        [MessageRequired, MessageMaxLength(500)]
        public string FileKey { get; set; } = string.Empty;

        // Tên file gốc hiển thị cho admin/student.
        [MessageRequired, MessageMaxLength(255)]
        public string FileName { get; set; } = string.Empty;

    }
}
