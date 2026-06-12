namespace Filters
{
    public class AuthAccountFilterDTO : FilterDTO
    {
        private const string CollectionFilterMarker = "[].";
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(AuthAccount.Id),
                ["userId"] = nameof(AuthAccount.UserIdText),
                ["userIdText"] = nameof(AuthAccount.UserIdText),
                ["email"] = nameof(AuthAccount.Email),
                ["fullName"] = nameof(AuthAccount.User) + "." + nameof(User.FullName),
                ["phoneNumber"] = nameof(AuthAccount.User) + "." + nameof(User.PhoneNumber),
                ["status"] = nameof(AuthAccount.Status),
                ["gender"] = nameof(AuthAccount.User) + "." + nameof(User.Gender),
                ["createdAt"] = nameof(AuthAccount.CreationDate),
                ["updatedAt"] = nameof(AuthAccount.ModificationDate)
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.Contains)]
        [SearchField(nameof(AuthAccount.UserIdText))]
        public string? UserIdText { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        [SearchField(nameof(AuthAccount.Email))]
        public string? Email { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(AuthAccount.User) + "." + nameof(User.FullName))]
        [SearchField(nameof(AuthAccount.User) + "." + nameof(User.FullName))]
        public string? FullName { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(AuthAccount.User) + "." + nameof(User.PhoneNumber))]
        [SearchField(nameof(AuthAccount.User) + "." + nameof(User.PhoneNumber))]
        public string? PhoneNumber { get; set; }

        [FilterField]
        public AuthAccountStatus? Status { get; set; }

        [FilterField(TargetField: nameof(AuthAccount.User) + "." + nameof(User.Gender))]
        public UserGender? Gender { get; set; }

        [FilterField(TargetField: nameof(AuthAccount.User) + "." + nameof(User.UserRoles) + CollectionFilterMarker + nameof(UserRole.Role) + "." + nameof(UserRole.Role.Name))]
        public RoleEnum? Role { get; set; }
    }
}
