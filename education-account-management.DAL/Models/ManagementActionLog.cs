namespace Models
{
    public class ManagementActionLog : BaseEntity
    {
        public Guid BatchId { get; set; }

        [MessageRequired, MessageMaxLength(80)]
        public string EntityType { get; set; } = string.Empty;

        public int EntityId { get; set; }

        [MessageRequired, MessageMaxLength(80)]
        public string Action { get; set; } = string.Empty;

        [MessageMaxLength(80)]
        public string? PreviousStatus { get; set; }

        [MessageMaxLength(80)]
        public string? NewStatus { get; set; }

        [MessageRequired, MessageMinLength(10), MessageMaxLength(500)]
        public string Reason { get; set; } = string.Empty;

        public int? ActorUserId { get; set; }
        public User? ActorUser { get; set; }

        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

        [MessageRequired, MessageMaxLength(45)]
        public string IpAddress { get; set; } = string.Empty;

        [MessageMaxLength(4000)]
        public string? MetadataJson { get; set; }
    }
}
