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
        [SearchField("SchoolStudent.EducationAccount.AccountNumber")]
        [SearchField("SchoolStudent.EducationAccount.Citizen.FullName")]
        [SearchField("FasScheme.SchemeName")]
        public Enums.FasApplicationStatus? Status { get; set; }
    }
}
