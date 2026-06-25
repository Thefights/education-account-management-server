namespace DTOs.FasSchemes
{
    public sealed class FasSchemeTierDTO
    {
        public int Id { get; set; }
        public string TierName { get; set; } = string.Empty;
        public decimal? MaxPerCapitaIncome { get; set; }
        public decimal? SubsidyValue { get; set; }
        public decimal? CourseFeeSubsidyValue { get; set; }
        public decimal? MiscFeeSubsidyValue { get; set; }
        public int DisplayOrder { get; set; }
    }

    public sealed class FasSchemeTierRequestDTO
    {
        [MessageRequired, MessageMaxLength(100)]
        public string TierName { get; set; } = string.Empty;

        public decimal? MaxPerCapitaIncome { get; set; }
        public decimal? SubsidyValue { get; set; }
        public decimal? CourseFeeSubsidyValue { get; set; }
        public decimal? MiscFeeSubsidyValue { get; set; }

        public int DisplayOrder { get; set; }
    }
}
