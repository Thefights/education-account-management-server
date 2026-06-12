using EntityAnnotations.RegExAttributes;

namespace Models
{
    public class SocialLogin : BaseEntity
    {
        [NotDefaultValue]
        public int AuthAccountId { get; set; }
        public AuthAccount AuthAccount { get; set; } = null!;

        [EnumDefined]
        public SocialLoginProvider Provider { get; set; }

        [MessageRequired, MessageMaxLength(256)]
        public string ProviderUserId { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(320), EmailValidator]
        public string ProviderEmail { get; set; } = string.Empty;

        public bool EmailVerified { get; set; }

        public DateTime LinkedAt { get; set; }
    }
}
