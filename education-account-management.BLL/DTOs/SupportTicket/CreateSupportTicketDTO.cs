using System.ComponentModel.DataAnnotations;

namespace DTOs.SupportTicket
{
    public class CreateSupportTicketDTO
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string QuestionMessage { get; set; } = string.Empty;
    }
}
