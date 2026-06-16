using Common;

namespace Models
{
    public class OtpVerification : AuditEntity
    {
        [MessageRequired, MessageMaxLength(100), Unique]
        public string SessionId { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(500)]
        public string OtpHash { get; set; } = string.Empty;

        [NumberPositive]
        public int FailedAttemptCount { get; set; }

        public DateTime ExpiresAt { get; set; }

        [NotDefaultValue]
        public int AuthAccountId { get; set; }
        public AuthAccount AuthAccount { get; set; } = null!;
    }
}
