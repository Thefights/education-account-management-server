namespace Models
{
    public class FasApplication : AuditEntity
    {
        // Scheme mà student đang apply.
        [NotDefaultValue]
        public int FasSchemeId { get; set; }
        public FasScheme FasScheme { get; set; } = null!;

        // Student thuộc school đang nộp application.
        [NotDefaultValue]
        public int SchoolStudentId { get; set; }
        public SchoolStudent SchoolStudent { get; set; } = null!;

        // Tier được hệ thống đề xuất sau khi evaluate eligibility.
        public int? RecommendedTierId { get; set; }
        public FasSchemeTier? RecommendedTier { get; set; }

        // Tier cuối cùng được admin approve; có thể khác recommended tier.
        public int? ApprovedTierId { get; set; }
        public FasSchemeTier? ApprovedTier { get; set; }

        // Mã application hiển thị cho admin/student tra cứu.
        [MessageRequired, MessageMaxLength(30), Unique]
        public string ApplicationNumber { get; set; } = string.Empty;

        // Trạng thái quyết định của application.
        [EnumDefined]
        public FasApplicationStatus Status { get; set; } = FasApplicationStatus.Pending;

        // Tuổi học sinh tại thời điểm apply, dùng làm snapshot eligibility.
        [NumberPositive]
        public int StudentAgeSnapshot { get; set; }

        // Quốc tịch học sinh tại thời điểm apply.
        [NotDefaultValue]
        public int StudentNationalityId { get; set; }
        public Country StudentNationality { get; set; } = null!;

        // Quốc tịch phụ huynh tại thời điểm apply.
        [NotDefaultValue]
        public int ParentNationalityId { get; set; }
        public Country ParentNationality { get; set; } = null!;

        // Tổng thu nhập hộ gia đình hằng tháng tại thời điểm apply.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal GrossHouseholdIncomeSnapshot { get; set; }

        // Số thành viên hộ gia đình tại thời điểm apply.
        [NumberHigherThan(0)]
        public int HouseholdMemberCountSnapshot { get; set; }

        // PCI đã tính tại thời điểm apply.
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal PerCapitaIncomeSnapshot { get; set; }

        // Lý do/rule matched khiến system recommend tier.
        [MessageMaxLength(1000)]
        public string? RecommendationReason { get; set; }

        // Lý do reject do admin nhập.
        [MessageMaxLength(1000)]
        public string? RejectionReason { get; set; }

        // Thời điểm application được approve.
        public DateTime? ApprovedAt { get; set; }

        // User admin đã approve application.
        public int? ApprovedByUserId { get; set; }
        public User? ApprovedByUser { get; set; }

        // Duration của scheme được snapshot tại thời điểm approve.
        [DurationInMonths]
        public int? DurationInMonthsSnapshot { get; set; }

        // Ngày bắt đầu hiệu lực của approved FAS.
        public DateTime? ValidityStartDate { get; set; }

        // Ngày hết hiệu lực của approved FAS.
        public DateTime? ValidityEndDate { get; set; }

        // Thời điểm student rút application pending.
        public DateTime? WithdrawnAt { get; set; }

        // Giấy tờ student đã upload cho application.
        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<FasApplicationDocument> Documents { get; set; } = [];

        // Lịch sử admin override tier trong lúc review.
        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<FasTierOverrideHistory> TierOverrideHistories { get; set; } = [];

        // Các charge đã dùng application này làm best-choice FAS.
        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<Charge> AppliedCharges { get; set; } = [];
    }
}
