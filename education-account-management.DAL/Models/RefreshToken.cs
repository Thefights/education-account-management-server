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
        public int AuthAccountId { get; set; }
        public AuthAccount AuthAccount { get; set; } = null!;
    }
}
