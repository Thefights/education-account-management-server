namespace DTOs.FasApplications
{
    public class ApproveFasApplicationDTO
    {
        public int? ApprovedTierId { get; set; }

        [MessageMaxLength(500)]
        public string? Reason { get; set; }
    }
}
