namespace Models
{
    public class FasTierOverrideHistory : AuditEntity
    {
        // Application được admin override tier.
        [NotDefaultValue]
        public int FasApplicationId { get; set; }
        public FasApplication FasApplication { get; set; } = null!;

        // Tier cũ trước khi override; thường là recommended tier.
        public int? OldTierId { get; set; }
        public FasSchemeTier? OldTier { get; set; }

        // Tier mới sau khi admin override.
        [NotDefaultValue]
        public int NewTierId { get; set; }
        public FasSchemeTier NewTier { get; set; } = null!;

        // User admin thực hiện override.
        [NotDefaultValue]
        public int ModifiedByUserId { get; set; }
        public User ModifiedByUser { get; set; } = null!;

        // Thời điểm admin override tier.
        [NotDefaultValue]
        public DateTime ModifiedAt { get; set; }

        // Lý do bắt buộc khi admin override tier.
        [MessageRequired, MessageMaxLength(1000)]
        public string Reason { get; set; } = string.Empty;
    }
}
