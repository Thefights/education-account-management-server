namespace Models
{
    public class FasSchemeCondition : AuditEntity
    {
        // Group điều kiện chứa condition này.
        [NotDefaultValue]
        public int GroupId { get; set; }
        public FasSchemeConditionGroup Group { get; set; } = null!;

        // Field nghiệp vụ được dùng để kiểm tra eligibility.
        [EnumDefined]
        public FasConditionField Field { get; set; }

        // Toán tử so sánh cho field eligibility.
        [EnumDefined]
        public FasConditionOperator Operator { get; set; }

        // Giá trị số dùng cho tuổi, income hoặc PCI.
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValueNumber { get; set; }

        // Giá trị số kết thúc dùng cho operator Between.
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValueNumberTo { get; set; }

        // Quốc gia dùng cho StudentNationality hoặc ParentNationality.
        public int? CountryId { get; set; }
        public Country? Country { get; set; }

        // Thứ tự hiển thị condition trong group.
        [NumberPositive]
        public int DisplayOrder { get; set; }
    }
}
