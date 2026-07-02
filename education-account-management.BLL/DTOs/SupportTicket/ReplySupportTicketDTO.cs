using System.ComponentModel.DataAnnotations;

namespace DTOs.SupportTicket
{
    public class ReplySupportTicketDTO
    {
        [Required]
        [MaxLength(2000)]
        public string ReplyMessage { get; set; } = string.Empty;
    }
}
