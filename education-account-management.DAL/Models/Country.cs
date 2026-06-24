namespace Models
{
    public class Country : AuditEntity
    {
        // Mã quốc gia ISO alpha-2 dùng để lưu nationality ổn định và dễ tích hợp.
        [MessageRequired, MessageMaxLength(2), Unique]
        public string Code { get; set; } = string.Empty;

        // Tên quốc gia hiển thị cho admin/student trong các dropdown nationality.
        [MessageRequired, MessageMaxLength(100), Unique]
        public string Name { get; set; } = string.Empty;

        // Cho biết quốc gia còn được chọn trong cấu hình/đơn FAS mới hay không.
        public bool IsActive { get; set; } = true;

        // Các điều kiện FAS đang dùng quốc gia này làm giá trị so sánh.
        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<FasSchemeCondition> FasSchemeConditions { get; set; } = [];

        // Các đơn FAS có snapshot quốc tịch học sinh là quốc gia này.
        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<FasApplication> StudentNationalityApplications { get; set; } = [];

        // Các đơn FAS có snapshot quốc tịch phụ huynh là quốc gia này.
        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<FasApplication> ParentNationalityApplications { get; set; } = [];
    }
}
