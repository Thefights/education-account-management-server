namespace Filters.TransactionHistory
{
    public class EducationCreditTransactionFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = nameof(EducationCreditTransaction.Id),
                ["transactionCode"] = nameof(EducationCreditTransaction.TransactionCode),
                ["createdAt"] = nameof(EducationCreditTransaction.CreatedAt),
                ["amount"] = nameof(EducationCreditTransaction.Amount),
                ["type"] = nameof(EducationCreditTransaction.Type),
                ["direction"] = nameof(EducationCreditTransaction.Direction),
                ["balanceBefore"] = nameof(EducationCreditTransaction.BalanceBefore),
                ["balanceAfter"] = nameof(EducationCreditTransaction.BalanceAfter),
                ["description"] = nameof(EducationCreditTransaction.Description),
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;

        public override string Sort { get; set; } = "createdAt desc";

        [FilterField(FilterOperationEnum.In, nameof(EducationCreditTransaction.Type))]
        public List<EducationCreditTransactionType>? Types { get; set; }

        [FilterField(FilterOperationEnum.In, nameof(EducationCreditTransaction.Direction))]
        public List<EducationCreditTransactionDirection>? Directions { get; set; }

        [FilterField(FilterOperationEnum.GreaterThanOrEqual, nameof(EducationCreditTransaction.CreatedAt))]
        public DateTime? CreatedFrom { get; set; }

        [FilterField(FilterOperationEnum.LessThanOrEqual, nameof(EducationCreditTransaction.CreatedAt))]
        public DateTime? CreatedTo { get; set; }

        [FilterField(FilterOperationEnum.Contains, nameof(EducationCreditTransaction.Description))]
        [SearchField(nameof(EducationCreditTransaction.Description))]
        public string? Description { get; set; }
    }
}
