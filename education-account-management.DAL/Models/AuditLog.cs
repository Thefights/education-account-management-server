namespace Models
{
    public class AuditLog : BaseEntity
    {
        public int? ActorUserId { get; set; }
        public User? ActorUser { get; set; }

        [MessageRequired, MessageMaxLength(100)]
        public string ActorFullName { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(256)]
        public string ActorUserIdText { get; set; } = string.Empty;

        [MessageMaxLength(500)]
        public string? Object { get; set; }

        [MessageRequired, MessageMaxLength(45)]
        public string IpAddress { get; set; } = string.Empty;

        [EnumDefined]
        public AuditLogCategory Category { get; set; }

        [EnumDefined]
        public AuditLogAction Action { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}