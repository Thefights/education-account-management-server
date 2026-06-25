using Enums;
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
                ["submittedAt"] = nameof(FasApplication.CreatedAt),
                ["approvedAt"] = nameof(FasApplication.ApprovedAt),
                ["status"] = nameof(FasApplication.Status),
                ["schemeName"] = "FasScheme.SchemeName"
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(TargetField: nameof(FasApplication.Status))]
        public FasApplicationStatus? Status { get; set; }
    }
}
