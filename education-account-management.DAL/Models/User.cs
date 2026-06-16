using Common;
using EntityAnnotations.OnDeleteAttributes;
using Enums;

namespace Models
{
    public class User : AuditEntity
    {
        [EnumDefined]
        public UserRole Role { get; set; } = UserRole.AccountHolder;

        [NotDefaultValue]
        public int AuthAccountId { get; set; }
        public AuthAccount AuthAccount { get; set; } = null!;

        public int? CitizenId { get; set; }
        public Citizen? Citizen { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public AdminProfile? AdminProfile { get; set; }
    }
}
