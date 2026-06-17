namespace Models
{
    public class TopupRuleCondition : AuditEntity
    {
        [EnumDefined]
        public TopupRuleConditionField Field { get; set; } = TopupRuleConditionField.Age;

        [EnumDefined]
        public TopupRuleConditionOperator Operator { get; set; } = TopupRuleConditionOperator.Equals;

        [MessageMaxLength(100)]
        public string? ValueText { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValueNumber { get; set; }

        [NotDefaultValue]
        public int TopupRuleId { get; set; }
        public TopupRule TopupRule { get; set; } = null!;
    }
}
