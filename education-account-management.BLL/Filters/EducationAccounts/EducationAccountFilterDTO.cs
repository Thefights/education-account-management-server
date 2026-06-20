

namespace Filters.EducationAccounts
{
    public class EducationAccountFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(EducationAccount.Id),
                ["accountNumber"] = nameof(EducationAccount.AccountNumber),
                ["balance"] = nameof(EducationAccount.EducationCreditBalance),
                ["status"] = nameof(EducationAccount.Status),
                ["createdDate"] = nameof(EducationAccount.OpenedAt),
                ["nric"] = "Citizen.Nric",
                ["name"] = "Citizen.FullName",
                ["dateOfBirth"] = "Citizen.DateOfBirth"
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        public string? Status { get; set; }

        [FilterField(FilterOperationEnum.Contains, "Citizen.Nric")]
        [SearchField("Citizen.Nric")]
        public string? Nric { get; set; }

        [FilterField(FilterOperationEnum.Contains, "Citizen.FullName")]
        [SearchField("Citizen.FullName")]
        public string? Name { get; set; }

        protected override string? BuildFilter()
        {
            var baseFilter = base.BuildFilter();
            var filters = string.IsNullOrWhiteSpace(baseFilter)
                ? []
                : baseFilter.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

            if (!string.IsNullOrWhiteSpace(Status))
            {
                if (string.Equals(Status, "Active", StringComparison.OrdinalIgnoreCase))
                {
                    filters.Add($"Status = {(int)EducationAccountStatus.Active}");
                }
                else if (string.Equals(Status, "Extended", StringComparison.OrdinalIgnoreCase))
                {
                    filters.Add($"Status = {(int)EducationAccountStatus.Extended}");
                }
                else if (string.Equals(Status, "Inactive", StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(Status, "Closed", StringComparison.OrdinalIgnoreCase))
                {
                    filters.Add($"Status = {(int)EducationAccountStatus.Closed}");
                }
            }

            return filters.Count != 0 ? string.Join(",", filters) : null;
        }
    }
}
