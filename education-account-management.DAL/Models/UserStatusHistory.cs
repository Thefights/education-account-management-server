namespace Models
{
    public class UserStatusHistory : BaseEntity
    {
        [NotDefaultValue]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [EnumDefined]
        public UserStatus PreviousStatus { get; set; }

        [EnumDefined]
        public UserStatus NewStatus { get; set; }

        [MessageRequired, MessageMaxLength(500)]
        public string Reason { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public int? ChangedByUserId { get; set; }
        public User? ChangedByUser { get; set; }
    }
}
