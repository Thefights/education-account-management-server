using Common;
using EntityAnnotations.OnDeleteAttributes;
using Enums;

namespace Models
{
    public class AuthAccount : AuditEntity
    {
        [EnumDefined]
        public AuthAccountStatus Status { get; set; } = AuthAccountStatus.Active;

        [NumberPositive]
        public int FailedLoginCount { get; set; }

        public DateTime? LockedUntil { get; set; }

        public DateTime? LastLoginAt { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<SsoIdentity> SsoIdentities { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<OtpVerification> OtpVerifications { get; set; } = [];
    }
}
