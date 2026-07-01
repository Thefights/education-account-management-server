namespace Models
{
    public class Notification : AuditEntity
    {
        [NotDefaultValue]
        public int RecipientUserId { get; set; }
        public User RecipientUser { get; set; } = null!;

        [EnumDefined]
        public NotificationType Type { get; set; }

        [EnumDefined]
        public NotificationSeverity Severity { get; set; } = NotificationSeverity.Info;

        [MessageRequired, MessageMaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(1000)]
        public string Message { get; set; } = string.Empty;

        [MessageMaxLength(100)]
        public string? RelatedEntityType { get; set; }

        public int? RelatedEntityId { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? MetadataJson { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadAt { get; set; }
    }
}