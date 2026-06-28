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

        public Enums.FasApplicationStatus? Status { get; set; }
    }
}
