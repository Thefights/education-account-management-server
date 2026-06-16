using Common;
using Enums;

namespace Models
{
    public class SsoIdentity : AuditEntity
    {
        [EnumDefined]
        public SsoProvider Provider { get; set; } = SsoProvider.Singpass;

        [MessageRequired, MessageMaxLength(256)]
        public string ProviderUserId { get; set; } = string.Empty;

        [NotDefaultValue]
        public int AuthAccountId { get; set; }
        public AuthAccount AuthAccount { get; set; } = null!;
    }
}
