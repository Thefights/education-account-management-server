namespace Models
{
    public class AiAssistantSetting : AuditEntity
    {
        // Bật/tắt AI chatbot trên e-Service Portal.
        public bool IsEnabled { get; set; }

        // Tax rate dùng chung khi generate charge; charge sẽ snapshot lại rate này.
        [Column(TypeName = "decimal(5,4)"), NumberPositive]
        public decimal TaxRate { get; set; } = 0.09m;
    }
}
