namespace Models
{
    public class FasSchemeTier : AuditEntity
    {
        // Scheme sở hữu tier này.
        [NotDefaultValue]
        public int FasSchemeId { get; set; }
        public FasScheme FasScheme { get; set; } = null!;

        // Tên tier hiển thị cho admin/student.
        [MessageRequired, MessageMaxLength(100)]
        public string TierName { get; set; } = string.Empty;

        [EnumDefined]
        public FasTierIncomeBasis TierIncomeBasis { get; set; } = FasTierIncomeBasis.PerCapitaIncome;

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal? MinPerCapitaIncome { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal? MaxPerCapitaIncome { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal? MinGrossHouseholdIncome { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal? MaxGrossHouseholdIncome { get; set; }

        // Giá trị hỗ trợ chung khi scheme không tách Course Fee/Misc Fee.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal? SubsidyValue { get; set; }

        // Cho biết tier này có tách giá trị hỗ trợ riêng cho Course Fee và Misc Fee hay không.
        public bool IsPerComponent { get; set; }

        // Giá trị hỗ trợ riêng cho Course Fee khi tier bật IsPerComponent.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal? CourseFeeSubsidyValue { get; set; }

        // Giá trị hỗ trợ riêng cho Misc Fee khi tier bật IsPerComponent.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal? MiscFeeSubsidyValue { get; set; }

        // Thứ tự hiển thị tier trong bảng tier.
        [NumberPositive]
        public int DisplayOrder { get; set; }

        // Applications được system recommend tier này.
        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<FasApplication> RecommendedApplications { get; set; } = [];

        // Applications được admin approve bằng tier này.
        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<FasApplication> ApprovedApplications { get; set; } = [];
    }
}
