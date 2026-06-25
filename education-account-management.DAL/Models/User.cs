namespace Models
{
    public class User : AuditEntity
    {
        [EnumDefined]
        public UserRole Role { get; set; } = UserRole.AccountHolder;

        [EnumDefined]
        public UserStatus Status { get; set; } = UserStatus.Active;

        [NumberPositive]
        public int FailedLoginCount { get; set; }

        public DateTime? LockedUntil { get; set; }

        public DateTime? LastLoginAt { get; set; }

        [Unique]
        public int? CitizenId { get; set; }
        public Citizen? Citizen { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public AdminProfile? AdminProfile { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<SsoIdentity> SsoIdentities { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<UserStatusHistory> StatusHistories { get; set; } = [];
    }
}
