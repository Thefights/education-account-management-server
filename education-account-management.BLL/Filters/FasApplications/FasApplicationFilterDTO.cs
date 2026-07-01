using Filters.Base;
using Models;

namespace Filters.FasApplications
{
    public class FasApplicationFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(FasApplication.Id),
                ["applicationNumber"] = nameof(FasApplication.ApplicationNumber),
                ["createdAt"] = nameof(FasApplication.CreatedAt),
                ["approvedAt"] = nameof(FasApplication.ApprovedAt),
                ["status"] = nameof(FasApplication.Status),
                ["schemeName"] = $"{nameof(FasApplication.FasScheme)}.{nameof(FasScheme.SchemeName)}",
                ["accountName"] = $"{nameof(FasApplication.SchoolStudent)}.{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.FullName)}",
                ["accountNumber"] = $"{nameof(FasApplication.SchoolStudent)}.{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.AccountNumber)}",
                ["submittedAt"] = nameof(FasApplication.CreatedAt)
        };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;
        public override string Sort { get; set; } = "createdAt desc";

        [SearchField(nameof(FasApplication.ApplicationNumber))]
        [FilterField(FilterOperationEnum.Contains, nameof(FasApplication.ApplicationNumber))]
        public string? ApplicationNumber { get; set; }

        [SearchField($"{nameof(FasApplication.SchoolStudent)}.{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.AccountNumber)}")]
        [FilterField(FilterOperationEnum.Contains, $"{nameof(FasApplication.SchoolStudent)}.{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.AccountNumber)}")]
        public string? AccountNumber { get; set; }

        [SearchField($"{nameof(FasApplication.SchoolStudent)}.{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.FullName)}")]
        [FilterField(FilterOperationEnum.Contains, $"{nameof(FasApplication.SchoolStudent)}.{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.FullName)}")]
        public string? AccountName { get; set; }

        [SearchField($"{nameof(FasApplication.FasScheme)}.{nameof(FasScheme.SchemeName)}")]
        [FilterField(FilterOperationEnum.Contains, $"{nameof(FasApplication.FasScheme)}.{nameof(FasScheme.SchemeName)}")]
        public string? SchemeName { get; set; }

        [FilterField(FilterOperationEnum.In, nameof(FasApplication.Status))]
        public List<FasApplicationStatus>? Statuses { get; set; }

        [FilterField(FilterOperationEnum.GreaterThanOrEqual, nameof(FasApplication.CreatedAt))]
        public DateTime? SubmittedFrom { get; set; }

        [FilterField(FilterOperationEnum.LessThanOrEqual, nameof(FasApplication.CreatedAt))]
        public DateTime? SubmittedTo { get; set; }

        [FilterField(FilterOperationEnum.Equal, nameof(FasApplication.FasSchemeId))]
        public int? SchemeId { get; set; }
    }
}
