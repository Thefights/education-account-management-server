namespace Models
{
    public class FasSchemeConditionGroup : AuditEntity
    {
        // Scheme sở hữu group điều kiện này.
        [NotDefaultValue]
        public int FasSchemeId { get; set; }
        public FasScheme FasScheme { get; set; } = null!;

        // Group cha để hỗ trợ cây điều kiện lồng nhau.
        public int? ParentGroupId { get; set; }
        public FasSchemeConditionGroup? ParentGroup { get; set; }

        // Toán tử nối các điều kiện/group con trong group này: AND hoặc OR.
        [EnumDefined]
        public TopupLogicalOperator LogicalOperator { get; set; } = TopupLogicalOperator.And;

        // Thứ tự hiển thị group trong cây điều kiện.
        [NumberPositive]
        public int DisplayOrder { get; set; }

        // Các group con nằm dưới group hiện tại.
        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<FasSchemeConditionGroup> ChildGroups { get; set; } = [];

        // Các điều kiện trực tiếp thuộc group hiện tại.
        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<FasSchemeCondition> Conditions { get; set; } = [];
    }
}
