namespace Filters.SchoolStudents
{
    public class SchoolStudentFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
    new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["id"] = nameof(SchoolStudent.Id),

        ["accountNumber"] =
            $"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.AccountNumber)}",

        ["email"] =
            $"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.Email)}",

        ["phoneNumber"] =
            $"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.PhoneNumber)}",

        ["nric"] =
            $"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.Nric)}",

        ["fullName"] =
            $"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.FullName)}",

        ["courses"] =
            $"{nameof(SchoolStudent.Enrollments)}.Min({nameof(Enrollment.Course)}.{nameof(Course.CourseCode)})",

        ["status"] = nameof(SchoolStudent.Status),
        ["createdAt"] = nameof(SchoolStudent.CreatedAt),
    };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.Equal, nameof(SchoolStudent.SchoolId))]
        public int SchoolId { get; set; }

        [FilterField(FilterOperationEnum.In, nameof(SchoolStudent.Status))]
        public List<SchoolStudentStatus>? Statuses { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.Nric)}")]
        [SearchField($"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.Nric)}")]
        public string? Nric { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.FullName)}")]
        [SearchField($"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.FullName)}")]
        public string? FullName { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.Email)}")]
        [SearchField($"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.Email)}")]
        public string? Email { get; set; }

        [FilterField(FilterOperationEnum.Contains, $"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.PhoneNumber)}")]
        [SearchField($"{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.PhoneNumber)}")]
        public string? PhoneNumber { get; set; }
    }
}
