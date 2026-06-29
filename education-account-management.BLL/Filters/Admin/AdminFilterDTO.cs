namespace Filters.Admin
{
    public class AdminFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(User.Id),
                ["userId"] = nameof(User.Id),
                ["role"] = nameof(User.Role),
                ["status"] = nameof(User.Status),
                ["createdAt"] = nameof(User.CreatedAt),
                ["staffCode"] = $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.StaffCode)}",
                ["fullName"] = $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.FullName)}",
                ["nric"] = $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.Nric)}",
                ["email"] = $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.Email)}",
                ["phoneNumber"] = $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.PhoneNumber)}",
                ["schoolName"] = $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.School)}.{nameof(School.SchoolName)}",
                ["azureObjectId"] = $"{nameof(User.SsoIdentities)}.Min({nameof(SsoIdentity.ProviderUserId)})",
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.In, nameof(User.Role))]
        public List<UserRole>? Roles { get; set; }

        [FilterField(FilterOperationEnum.In, nameof(User.Status))]
        public List<UserStatus>? Statuses { get; set; }

        [FilterField(FilterOperationEnum.In, $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.SchoolId)}")]
        public List<int>? SchoolIds { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.StaffCode)}")]
        [SearchField($"{nameof(User.AdminProfile)}.{nameof(AdminProfile.StaffCode)}")]
        public string? StaffCode { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.FullName)}")]
        [SearchField($"{nameof(User.AdminProfile)}.{nameof(AdminProfile.FullName)}")]
        public string? FullName { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.Nric)}")]
        [SearchField($"{nameof(User.AdminProfile)}.{nameof(AdminProfile.Nric)}")]
        public string? Nric { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(User.AdminProfile)}.{nameof(AdminProfile.Email)}")]
        [SearchField($"{nameof(User.AdminProfile)}.{nameof(AdminProfile.Email)}")]
        public string? Email { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(User.SsoIdentities)}[].{nameof(SsoIdentity.ProviderUserId)}")]
        public string? AzureObjectId { get; set; }
    }
}
