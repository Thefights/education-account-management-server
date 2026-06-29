namespace DTOs.ApplicationSettings
{
    public class GetApplicationSettingDTO
    {
        public bool IsAiFeatureEnabled { get; set; }

        public decimal TaxRate { get; set; }

        public int InstallmentDueDay { get; set; }
    }

    public class UpdateApplicationSettingDTO
    {
        public bool IsAiFeatureEnabled { get; set; }

        public decimal TaxRate { get; set; }

        public int InstallmentDueDay { get; set; }
    }
}
