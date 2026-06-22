namespace Models
{
    public class OutboxMessage : BaseEntity
    {
        [MessageRequired, MessageMaxLength(50)]
        public string Type { get; set; } = string.Empty;

        [MessageRequired]
        [Column(TypeName = "nvarchar(max)")]
        public string PayloadJson { get; set; } = string.Empty;

        [EnumDefined]
        public OutboxMessageStatus Status { get; set; } = OutboxMessageStatus.Pending;

        [NumberPositive]
        public int RetryCount { get; set; }

        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
