namespace Models
{
    public class AuditLog : BaseEntity
    {
        [EnumDefined]
        public AuditLogCategory Category { get; set; } = AuditLogCategory.Security;

        [MessageRequired, MessageMaxLength(100)]
        public string Action { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(45)]
        public string IpAddress { get; set; } = string.Empty;

        [Column(TypeName = "json")]
        public string? PayloadJson { get; set; }

        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

        public int? ActorUserId { get; set; }
        public User? ActorUser { get; set; }
    }
}
