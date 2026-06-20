using System.ComponentModel.DataAnnotations;

namespace DTOs.TopUp
{
    // ── Condition response ──────────────────────────────────────────────────

    public class TopupRuleConditionDTO
    {
        public int Id { get; set; }

        public string? Field { get; set; }

        public string? Operator { get; set; }

        public string? ValueText { get; set; }

        public decimal? ValueNumber { get; set; }

        public decimal? ConditionAmount { get; set; }

        public int DisplayOrder { get; set; }
    }

    // ── Condition requests ──────────────────────────────────────────────────

    public class CreateTopupRuleConditionDTO
    {
        [EnumDefined]
        public TopupRuleConditionField Field { get; set; }

        [EnumDefined]
        public TopupRuleConditionOperator Operator { get; set; }

        public string? ValueText { get; set; }

        public decimal? ValueNumber { get; set; }

        public decimal? ConditionAmount { get; set; }

        [MessageRange(0, int.MaxValue)]
        public int DisplayOrder { get; set; }
    }

    public class UpdateTopupRuleConditionDTO
    {
        public int? Id { get; set; }

        [EnumDefined]
        public TopupRuleConditionField Field { get; set; }

        [EnumDefined]
        public TopupRuleConditionOperator Operator { get; set; }

        public string? ValueText { get; set; }

        public decimal? ValueNumber { get; set; }

        public decimal? ConditionAmount { get; set; }

        [MessageRange(0, int.MaxValue)]
        public int DisplayOrder { get; set; }
    }

    // ── Rule response ───────────────────────────────────────────────────────

    public class GetTopupRuleDTO
    {
        public int Id { get; set; }

        public string RuleName { get; set; } = string.Empty;

        public string? Type { get; set; }

        public string? MatchMode { get; set; }

        public decimal? TopupAmount { get; set; }

        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<TopupRuleConditionDTO> Conditions { get; set; } = [];
    }

    // ── Rule requests ───────────────────────────────────────────────────────

    public class CreateTopupRuleDTO
    {
        [MessageRequired]
        [MessageMaxLength(150)]
        public string RuleName { get; set; } = string.Empty;

        [EnumDefined]
        public TopupRuleType Type { get; set; } = TopupRuleType.System;

        [EnumDefined]
        public TopupMatchMode MatchMode { get; set; } = TopupMatchMode.And;

        public decimal? TopupAmount { get; set; }

        [MessageMinLength(1)]
        public List<CreateTopupRuleConditionDTO> Conditions { get; set; } = [];
    }

    public class UpdateTopupRuleDTO
    {
        [MessageRequired]
        [MessageMaxLength(150)]
        public string RuleName { get; set; } = string.Empty;

        [EnumDefined]
        public TopupRuleType Type { get; set; }

        [EnumDefined]
        public TopupMatchMode MatchMode { get; set; }

        public decimal? TopupAmount { get; set; }

        [EnumDefined]
        public TopupRuleStatus Status { get; set; }

        [MessageMinLength(1)]
        public List<UpdateTopupRuleConditionDTO> Conditions { get; set; } = [];
    }

    // ── Batch command ───────────────────────────────────────────────────────

    public class BatchUpdateTopupRuleStatusDTO
    {
        [MessageMinLength(1)]
        public List<int> Ids { get; set; } = [];

        [EnumDefined]
        public TopupRuleStatus Status { get; set; }
    }
}
