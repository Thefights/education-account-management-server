namespace Filters
{
    public class SchoolManagementFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(School.Id),
                ["schoolName"] = nameof(School.SchoolName),
                ["status"] = nameof(School.Status),
                ["email"] = nameof(School.Email),
                ["phoneNumber"] = nameof(School.PhoneNumber),
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.In, nameof(School.Status))]
        public List<SchoolStatus>? Statuses { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(School.SchoolName))]
        [SearchField(nameof(School.SchoolName))]
        public string? SchoolName { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(School.Email))]
        [SearchField(nameof(School.Email))]
        public string? Email { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(School.PhoneNumber))]
        [SearchField(nameof(School.PhoneNumber))]
        public string? PhoneNumber { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(School.Address))]
        [SearchField(nameof(School.Address))]
        public string? Address { get; set; }
    }
}