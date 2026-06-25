using System.ComponentModel.DataAnnotations;

namespace DTOs.FasApplications
{
    public class RejectFasApplicationDTO
    {
        [Required]
        [MaxLength(1000)]
        public string RejectionReason { get; set; } = string.Empty;
    }
}
