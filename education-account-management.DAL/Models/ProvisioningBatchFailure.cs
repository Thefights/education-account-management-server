namespace Models
{
    public class ProvisioningBatchFailure : BaseEntity
    {
        [NotDefaultValue]
        public int ProvisioningBatchReportId { get; set; }
        public ProvisioningBatchReport ProvisioningBatchReport { get; set; } = null!;

        [MessageRequired, MessageMaxLength(9), SingaporeNric]
        public string Nric { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(1000)]
        public string Reason { get; set; } = string.Empty;
    }
}
