namespace Models
{
    public class FasScheme : AuditEntity
    {
        // Trường sở hữu chương trình FAS; admin chỉ quản lý scheme thuộc trường của mình.
        [NotDefaultValue]
        public int SchoolId { get; set; }
        public School School { get; set; } = null!;

        // Trạng thái vòng đời của scheme: Draft, Active hoặc Inactive.
        [EnumDefined]
        public FasSchemeStatus Status { get; set; } = FasSchemeStatus.Draft;

        // Mã scheme tự sinh để admin/student tra cứu.
        [MessageRequired, MessageMaxLength(20), Unique]
        public string SchemeCode { get; set; } = string.Empty;

        // Tên chương trình FAS hiển thị trên Admin Portal và e-Service.
        [MessageRequired, MessageMaxLength(150)]
        public string SchemeName { get; set; } = string.Empty;

        // Mô tả ngắn về mục tiêu/đối tượng của chương trình FAS.
        [MessageMaxLength(1000)]
        public string? Description { get; set; }

        // Số tháng hiệu lực áp dụng cho từng application sau khi được duyệt.
        [DurationInMonths]
        public int DurationInMonths { get; set; }

        // Cách tính hỗ trợ của scheme: phần trăm hoặc số tiền cố định.
        [EnumDefined]
        public FasSubsidyType SubsidyType { get; set; } = FasSubsidyType.Percent;

        // Cho biết tier có tách giá trị hỗ trợ riêng cho Course Fee và Misc Fee hay không.
        public bool IsPerComponent { get; set; }

        // Thời điểm scheme được publish lần đầu; scheme không có ngày hết hạn.
        public DateTime? PublishedAt { get; set; }

        // Các group điều kiện eligibility; root group là group không có ParentGroupId.
        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<FasSchemeConditionGroup> ConditionGroups { get; set; } = [];

        // Danh sách tier hỗ trợ của scheme.
        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<FasSchemeTier> Tiers { get; set; } = [];

        // Danh sách giấy tờ bắt buộc student phải upload khi apply.
        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<FasSchemeRequiredDocument> RequiredDocuments { get; set; } = [];

        // Danh sách câu hỏi bổ sung student phải trả lời khi apply.
        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<FasSchemeAdditionalQuestion> AdditionalQuestions { get; set; } = [];

        // Danh sách khóa học mà scheme được phép áp dụng.
        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<FasSchemeCourse> SchemeCourses { get; set; } = [];

        // Danh sách application student đã nộp cho scheme này.
        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<FasApplication> Applications { get; set; } = [];
    }
}
