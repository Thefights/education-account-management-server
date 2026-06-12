namespace Models
{
    public class PasswordResetToken : BaseEntity
    {
        [NotDefaultValue]
        public int AuthAccountId { get; set; }
        public AuthAccount AuthAccount { get; set; } = null!;

        [MessageRequired, MessageMaxLength(500), Unique]
        public string TokenHash { get; set; } = string.Empty;

        [MessageMaxLength(45)]
        public string? RequestedByIp { get; set; }

        [MessageMaxLength(300)]
        public string? UserAgent { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime? UsedAt { get; set; }
    }
}
