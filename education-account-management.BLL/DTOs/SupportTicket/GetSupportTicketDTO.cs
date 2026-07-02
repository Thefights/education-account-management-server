using Enums;
using System;

namespace DTOs.SupportTicket
{
    public class GetSupportTicketDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string QuestionMessage { get; set; } = string.Empty;
        public string? AdminResponse { get; set; }
        public SupportTicketStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string AccountHolderName { get; set; } = string.Empty;
    }
}
