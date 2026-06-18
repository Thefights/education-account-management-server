namespace Filters
{
    public class AdminManagementFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(User.Id),
                ["userId"] = nameof(User.Id),
                ["role"] = nameof(User.Role),
                ["status"] = $"{nameof(User.AuthAccount)}.{nameof(AuthAccount.Status)}",
                ["staffCode"] = $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.StaffCode)}",
                ["fullName"] = $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.FullName)}",
                ["email"] = $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.Email)}",
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.In, nameof(User.Role))]
        public List<UserRole>? Roles { get; set; }

        [FilterField(FilterOperationEnum.In, $"{nameof(User.AuthAccount)}.{nameof(AuthAccount.Status)}")]
        public List<AuthAccountStatus>? Statuses { get; set; }

        [FilterField(FilterOperationEnum.Equal, $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.SchoolId)}")]
        public int? SchoolId { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.StaffCode)}")]
        [SearchField($"{nameof(User.AdminProfile)}.{nameof(AdminProfile.StaffCode)}")]
        public string? StaffCode { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.FullName)}")]
        [SearchField($"{nameof(User.AdminProfile)}.{nameof(AdminProfile.FullName)}")]
        public string? FullName { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.Email)}")]
        [SearchField($"{nameof(User.AdminProfile)}.{nameof(AdminProfile.Email)}")]
        public string? Email { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(User.AuthAccount)}.{nameof(AuthAccount.SsoIdentities)}[].{nameof(SsoIdentity.ProviderUserId)}")]
        public string? AzureObjectId { get; set; }
    }
}
