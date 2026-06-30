namespace DTOs.FasApplications
{
    public class ApproveFasApplicationDTO
    {
        public int? ApprovedTierId { get; set; }

        public int? TierId { get; set; }

        [MessageMaxLength(500)]
        public string? Reason { get; set; }

        [MessageMaxLength(500)]
        public string? OverrideReason { get; set; }

        public int? SelectedTierId => ApprovedTierId ?? TierId;

        public string? EffectiveReason =>
            !string.IsNullOrWhiteSpace(Reason)
                ? Reason
                : OverrideReason;
    }
}
