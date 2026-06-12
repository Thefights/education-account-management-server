namespace Models
{
    public class OtpVerification : BaseEntity
    {
        [NotDefaultValue]
        public int? AuthAccountId { get; set; }
        public AuthAccount? AuthAccount { get; set; }

        [EnumDefined]
        public OtpVerificationPurpose Purpose { get; set; }

        [EnumDefined]
        public OtpVerificationDeliveryMethod DeliveryMethod { get; set; }

        [MessageRequired, MessageMaxLength(100), Unique]
        public string SessionId { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(320)]
        public string Target { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(500)]
        public string OtpHash { get; set; } = string.Empty;

        [NumberPositive]
        public int FailedAttemptCount { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime? UsedAt { get; set; }

        public DateTime? InvalidatedAt { get; set; }
    }
}
