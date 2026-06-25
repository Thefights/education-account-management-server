using Filters.Base;
using Models;

namespace Filters.FasSchemes
{
    public class FasSchemeFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(FasScheme.Id),
                ["schemeName"] = nameof(FasScheme.SchemeName),
                ["schemeCode"] = nameof(FasScheme.SchemeCode),
                ["status"] = nameof(FasScheme.Status),
                ["durationInMonths"] = nameof(FasScheme.DurationInMonths),
                ["createdAt"] = nameof(FasScheme.CreatedAt),
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.In, nameof(FasScheme.Status))]
        public List<FasSchemeStatus>? Statuses { get; set; }

        [FilterField(FilterOperationEnum.Equal, nameof(FasScheme.SchoolId))]
        public int? SchoolId { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(FasScheme.SchemeName))]
        [SearchField(nameof(FasScheme.SchemeName))]
        public string? SchemeName { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(FasScheme.SchemeCode))]
        [SearchField(nameof(FasScheme.SchemeCode))]
        public string? SchemeCode { get; set; }

        [NumberPositive]
        public decimal? GrossHouseholdIncome { get; set; }
        public int? HouseholdMemberCount { get; set; }
        public Enums.NationalityCategory? GuardianNationality { get; set; }
    }
}
