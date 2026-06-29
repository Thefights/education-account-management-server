

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
                ["createdAt"] = nameof(EducationAccount.CreatedAt),
                ["createdDate"] = nameof(EducationAccount.OpenedAt),
                ["nric"] = "Citizen.Nric",
                ["name"] = "Citizen.FullName",
                ["dateOfBirth"] = "Citizen.DateOfBirth"
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        [FilterField(FilterOperationEnum.In, nameof(EducationAccount.Status))]
        public List<EducationAccountStatus>? Statuses { get; set; }

        [FilterField(FilterOperationEnum.Contains, "Citizen.Nric")]
        [SearchField("Citizen.Nric")]
        public string? Nric { get; set; }

        [FilterField(FilterOperationEnum.Contains, "Citizen.FullName")]
        [SearchField("Citizen.FullName")]
        public string? Name { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(EducationAccount.AccountNumber))]
        [SearchField(nameof(EducationAccount.AccountNumber))]
        public string? AccountNumber { get; set; }
    }
}
