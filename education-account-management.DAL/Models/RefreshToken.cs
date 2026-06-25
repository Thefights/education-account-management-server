using Common;

namespace Models
{
    public class RefreshToken : AuditEntity
    {
        [MessageRequired, MessageMaxLength(500), Unique]
        public string TokenHash { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        [NotDefaultValue]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
