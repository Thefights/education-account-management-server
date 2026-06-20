namespace Models
{
    public class SsoIdentity : AuditEntity
    {
        [EnumDefined]
        public SsoProvider Provider { get; set; } = SsoProvider.Singpass;

        [MessageRequired, MessageMaxLength(256)]
        public string ProviderUserId { get; set; } = string.Empty;

        [NotDefaultValue]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
