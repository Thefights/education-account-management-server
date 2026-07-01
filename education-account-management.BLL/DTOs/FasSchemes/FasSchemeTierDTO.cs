namespace DTOs.FasSchemes
{
    public sealed class FasSchemeTierDTO
    {
        public int Id { get; set; }
        public string TierName { get; set; } = string.Empty;
        public FasTierIncomeBasis TierIncomeBasis { get; set; }
        public decimal? MinPerCapitaIncome { get; set; }
        public decimal? MaxPerCapitaIncome { get; set; }
        public decimal? MinGrossHouseholdIncome { get; set; }
        public decimal? MaxGrossHouseholdIncome { get; set; }
        public bool IsPerComponent { get; set; }
        public decimal? SubsidyValue { get; set; }
        public decimal? CourseFeeSubsidyValue { get; set; }
        public decimal? MiscFeeSubsidyValue { get; set; }
        public int DisplayOrder { get; set; }
    }

    public sealed class FasSchemeTierRequestDTO
    {
        [MessageRequired, MessageMaxLength(100)]
        public string TierName { get; set; } = string.Empty;

        [EnumDefined]
        public FasTierIncomeBasis TierIncomeBasis { get; set; } = FasTierIncomeBasis.PerCapitaIncome;

        public decimal? MinPerCapitaIncome { get; set; }
        public decimal? MaxPerCapitaIncome { get; set; }
        public decimal? MinGrossHouseholdIncome { get; set; }
        public decimal? MaxGrossHouseholdIncome { get; set; }
        public bool IsPerComponent { get; set; }
        public decimal? SubsidyValue { get; set; }
        public decimal? CourseFeeSubsidyValue { get; set; }
        public decimal? MiscFeeSubsidyValue { get; set; }

        public int DisplayOrder { get; set; }
    }
}
