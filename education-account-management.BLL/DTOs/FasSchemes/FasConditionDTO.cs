namespace DTOs.FasSchemes
{
    public sealed class FasConditionDTO
    {
        public int Id { get; set; }
        public string? Field { get; set; }
        public string? Operator { get; set; }
        public decimal? ValueNumber { get; set; }
        public decimal? ValueNumberTo { get; set; }
        public int? CountryId { get; set; }
        public int DisplayOrder { get; set; }
    }

    public sealed class FasConditionGroupDTO
    {
        public int Id { get; set; }
        public string? LogicalOperator { get; set; }
        public int DisplayOrder { get; set; }
        public List<FasConditionDTO> Conditions { get; set; } = [];
        public List<FasConditionGroupDTO> Groups { get; set; } = [];
    }

    public sealed class FasConditionRequestDTO
    {
        [EnumDefined]
        public FasConditionField Field { get; set; }

        [EnumDefined]
        public FasConditionOperator Operator { get; set; }

        public decimal? ValueNumber { get; set; }
        public decimal? ValueNumberTo { get; set; }
        public int? CountryId { get; set; }
        public int DisplayOrder { get; set; }
    }

    public sealed class FasConditionGroupRequestDTO
    {
        [EnumDefined]
        public TopupLogicalOperator LogicalOperator { get; set; }

        public int DisplayOrder { get; set; }
        public List<FasConditionRequestDTO> Conditions { get; set; } = [];
        public List<FasConditionGroupRequestDTO> Groups { get; set; } = [];
    }
}
