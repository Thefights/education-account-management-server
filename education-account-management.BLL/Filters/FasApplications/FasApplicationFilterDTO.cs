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
                ["schemeName"] = $"{nameof(FasApplication.FasScheme)}.{nameof(FasScheme.SchemeName)}"
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

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

        [FilterField(TargetField: nameof(FasApplication.Status))]
        public FasApplicationStatus? Status { get; set; }
    }
}
