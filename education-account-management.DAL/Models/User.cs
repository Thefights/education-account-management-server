using education_account_management.DAL.Models;
using EntityAnnotations.RegExAttributes;

namespace Models
{
    public class User : EntityWithImage
    {
        public int AuthAccountId { get; set; }
        public AuthAccount AuthAccount { get; set; } = null!;

        [EnumDefined]
        public UserGender Gender { get; set; }

        [MessageRequired, MessageMaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MessageMaxLength(16), PhoneNumberValidator, Unique]
        public string? PhoneNumber { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
