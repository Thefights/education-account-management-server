namespace Filters.TransactionHistory
{
    public class EducationCreditTransactionFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(EducationCreditTransaction.Id),
                ["createdAt"] = nameof(EducationCreditTransaction.CreatedAt),
                ["amount"] = nameof(EducationCreditTransaction.Amount),
                ["type"] = nameof(EducationCreditTransaction.Type),
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        public override string Sort { get; set; } = "createdAt desc";

        [FilterField(FilterOperationEnum.Equal, nameof(EducationCreditTransaction.Type))]
        public EducationCreditTransactionType? Type { get; set; }

        [FilterField(FilterOperationEnum.Equal, nameof(EducationCreditTransaction.Direction))]
        public EducationCreditTransactionDirection? Direction { get; set; }

        [FilterField(FilterOperationEnum.GreaterThanOrEqual, nameof(EducationCreditTransaction.CreatedAt))]
        public DateTime? CreatedFrom { get; set; }

        [FilterField(FilterOperationEnum.LessThanOrEqual, nameof(EducationCreditTransaction.CreatedAt))]
        public DateTime? CreatedTo { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(EducationCreditTransaction.Description))]
        [SearchField(nameof(EducationCreditTransaction.Description))]
        public string? Description { get; set; }
    }
}
