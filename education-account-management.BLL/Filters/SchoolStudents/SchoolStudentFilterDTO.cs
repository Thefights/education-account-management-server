namespace Filters.SchoolStudents
{
    public class SchoolStudentFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(SchoolStudent.Id),
                ["nric"] = "EducationAccount.Citizen.Nric",
                ["fullName"] = "EducationAccount.Citizen.FullName",
                ["status"] = nameof(SchoolStudent.Status),
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.Equal, nameof(SchoolStudent.SchoolId))]
        public int SchoolId { get; set; }

        [FilterField(FilterOperationEnum.In, nameof(SchoolStudent.Status))]
        public List<SchoolStudentStatus>? Statuses { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(SchoolStudent.EducationAccount.Citizen.Nric))]
        [SearchField(nameof(SchoolStudent.EducationAccount.Citizen.Nric))]
        public string? Nric { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(SchoolStudent.EducationAccount.Citizen.FullName))]
        [SearchField(nameof(SchoolStudent.EducationAccount.Citizen.FullName))]
        public string? FullName { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(SchoolStudent.EducationAccount.Citizen.Email))]
        [SearchField(nameof(SchoolStudent.EducationAccount.Citizen.Email))]
        public string? Email { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(SchoolStudent.EducationAccount.Citizen.PhoneNumber))]
        [SearchField(nameof(SchoolStudent.EducationAccount.Citizen.PhoneNumber))]
        public string? PhoneNumber { get; set; }
    }
}