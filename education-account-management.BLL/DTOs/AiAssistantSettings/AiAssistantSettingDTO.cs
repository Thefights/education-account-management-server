namespace DTOs.AiAssistantSettings
{
    public class GetAiAssistantSettingDTO
    {
        public bool IsEnabled { get; set; }

        public decimal TaxRate { get; set; }

        public int InstallmentDueDay { get; set; }
    }

    public class UpdateAiAssistantSettingDTO
    {
        public bool IsEnabled { get; set; }

        public decimal TaxRate { get; set; }

        public int InstallmentDueDay { get; set; }
    }
}
