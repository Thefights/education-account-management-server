namespace Models
{
    public class ApplicationSetting : AuditEntity
    {
        public bool IsAiFeatureEnabled { get; set; }

        [Column(TypeName = "decimal(5,4)"), NumberPositive]
        public decimal TaxRate { get; set; } = 0.09m;

        [MessageRange(1, 28)]
        public int InstallmentDueDay { get; set; } = 5;
    }
}
