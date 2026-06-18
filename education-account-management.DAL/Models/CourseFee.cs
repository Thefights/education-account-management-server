namespace Models
{
    public class CourseFee : AuditEntity
    {
        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal CourseFeeAmount { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal MiscFeeAmount { get; set; }

        [Column(TypeName = "decimal(18,2)"), NumberPositive]
        public decimal GstAmount { get; set; }

        [NotDefaultValue]
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}
