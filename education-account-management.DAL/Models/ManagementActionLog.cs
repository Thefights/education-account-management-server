namespace Models
{
    public class ManagementActionLog : BaseEntity
    {
        public Guid BatchId { get; set; }

        [EnumDefined]
        public ManagementActionEntityType EntityType { get; set; }

        public int EntityId { get; set; }

        [EnumDefined]
        public ManagementAction Action { get; set; }

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
    }
}
