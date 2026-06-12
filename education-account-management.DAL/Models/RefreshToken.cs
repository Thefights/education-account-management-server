namespace Models
{
    public class RefreshToken : BaseEntity
    {
        [NotDefaultValue]
        public int AuthAccountId { get; set; }
        public AuthAccount AuthAccount { get; set; } = null!;

        [NotDefaultValue]
        public int? ReplacedByRefreshTokenId { get; set; }
        public RefreshToken? ReplacedByRefreshToken { get; set; }

        [MessageRequired, MessageMaxLength(500), Unique]
        public string TokenHash { get; set; } = string.Empty;

        [MessageMaxLength(45)]
        public string? CreatedByIp { get; set; }

        [MessageMaxLength(45)]
        public string? RevokedByIp { get; set; }

        [MessageMaxLength(300)]
        public string? UserAgent { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public bool StaySignedIn { get; set; }
    }
}
