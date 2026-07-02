namespace Models
{
    public class SupportTicket : AuditEntity
    {
        public int AccountHolderId { get; set; }
        public EducationAccount AccountHolder { get; set; } = null!;

        [MessageRequired, MessageMaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(1000)]
        public string QuestionMessage { get; set; } = string.Empty;

        [MessageMaxLength(2000)]
        public string? AdminResponse { get; set; }

        public int? ResponsedBy { get; set; }

        [ForeignKey("ResponsedBy")]
        public User? ResponsedByUser { get; set; }

        [EnumDefined]
        public SupportTicketStatus Status { get; set; } = SupportTicketStatus.Pending;

        public DateTime? ResolvedAt { get; set; }
    }
}
