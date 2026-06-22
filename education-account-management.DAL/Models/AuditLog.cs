namespace Models
{
    public class AuditLog : BaseEntity
    {
        [EnumDefined]
        public AuditLogCategory Category { get; set; } = AuditLogCategory.Security;

        [MessageRequired, MessageMaxLength(100)]
        public string Action { get; set; } = string.Empty;

        public string? Nric { get; set; }

        [MessageRequired, MessageMaxLength(45)]
        public string IpAddress { get; set; } = string.Empty;

        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

        public int? ActorUserId { get; set; }
        public User? ActorUser { get; set; }
    }
}
