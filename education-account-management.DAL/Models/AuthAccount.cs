using EntityAnnotations.RegExAttributes;

namespace Models
{
    public class AuthAccount : AuditEntity
    {
        [MessageRequired, MessageMaxLength(256), UserIdTextValidator, Unique]
        public string UserIdText { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(320), EmailValidator]
        public string Email { get; set; } = string.Empty;

        [MessageMaxLength(500), PasswordValidator]
        public string? PasswordHash { get; set; }

        [EnumDefined]
        public AuthAccountStatus Status { get; set; } = AuthAccountStatus.Active;

        [NumberPositive]
        public int FailedLoginCount { get; set; } = 0;

        public DateTime? LockedUntil { get; set; }

        public DateTime? LastLoginAt { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public User User { get; set; } = null!;

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];

        [OnDelete(OnDeleteBehavior.SetNull)]
        public ICollection<OtpVerification> OtpVerifications { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<SocialLogin> SocialLogins { get; set; } = [];
    }
}

