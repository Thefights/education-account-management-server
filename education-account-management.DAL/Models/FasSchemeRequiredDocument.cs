namespace Models
{
    public class FasSchemeRequiredDocument : AuditEntity
    {
        // Scheme sở hữu cấu hình giấy tờ bắt buộc này.
        [NotDefaultValue]
        public int FasSchemeId { get; set; }
        public FasScheme FasScheme { get; set; } = null!;

        // Tên giấy tờ hiển thị cho student khi apply.
        [MessageRequired, MessageMaxLength(150)]
        public string DocumentName { get; set; } = string.Empty;

        // Storage key/path của template để student download.
        [MessageMaxLength(500)]
        public string? TemplateFileKey { get; set; }

        // Thứ tự hiển thị document trong form apply.
        [NumberPositive]
        public int DisplayOrder { get; set; }

        // Các file student đã upload tương ứng với loại giấy tờ này.
        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<FasApplicationDocument> ApplicationDocuments { get; set; } = [];
    }
}
