using System.ComponentModel.DataAnnotations;

namespace DTOs.FasApplications
{
    public class RejectFasApplicationDTO
    {
        [MessageRequired]
        [MessageMaxLength(1000)]
        public string ExternalRejectionReason { get; set; } = string.Empty;

        [MessageRequired]
        [MessageMaxLength(1000)]
        public string InternalRejectionReason { get; set; } = string.Empty;
    }
}
