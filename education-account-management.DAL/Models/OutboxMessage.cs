namespace Models
{
    public class OutboxMessage : BaseEntity
    {
        [EnumDefined]
        public OutboxMessageType Type { get; set; }

        [EnumDefined]
        public OutboxMessageStatus Status { get; set; } = OutboxMessageStatus.Pending;

        [MessageRequired]
        public string PayloadJson { get; set; } = string.Empty;

        public int AttemptCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? NextAttemptAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        [MessageMaxLength(1000)]
        public string? LastError { get; set; }
    }
}
