namespace Models
{
    public class EducationAccountStatusHistory : BaseEntity
    {
        [NotDefaultValue]
        public int EducationAccountId { get; set; }
        public EducationAccount EducationAccount { get; set; } = null!;

        [EnumDefined]
        public EducationAccountStatus PreviousStatus { get; set; }

        [EnumDefined]
        public EducationAccountStatus NewStatus { get; set; }

        [MessageRequired, MessageMaxLength(500)]
        public string Reason { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public int? ChangedByUserId { get; set; }
        public User? ChangedByUser { get; set; }
    }
}
