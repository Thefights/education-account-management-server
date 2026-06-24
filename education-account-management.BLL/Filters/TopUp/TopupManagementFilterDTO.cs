namespace Filters.TopUp
{
    public sealed class TopupExecutionFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = "Id",
                ["executionCode"] = "ExecutionCode",
                ["topupNameSnapshot"] = "TopupNameSnapshot",
                ["sourceType"] = "SourceType",
                ["status"] = "Status",
                ["totalTargetCount"] = "TotalTargetCount",
                ["successCount"] = "SuccessCount",
                ["failedCount"] = "FailedCount",
                ["totalExecutedAmount"] = "TotalExecutedAmount",
                ["createdAt"] = "CreatedAt"
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;
        public List<TopupExecutionSourceType>? SourceTypes { get; set; }
        public List<TopupExecutionStatus>? Statuses { get; set; }
        public string? AccountNumber { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }

    public sealed class TopupExecutionTargetFilterDTO : FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> AllowedSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = "Id",
                ["accountNumber"] = "AccountNumber",
                ["accountName"] = "EducationAccount.Citizen.FullName",
                ["amount"] = "Amount",
                ["status"] = "Status",
                ["transactionCode"] = "EducationCreditTransaction.TransactionCode",
                ["failureReason"] = "FailureReason",
                ["createdAt"] = "CreatedAt"
            };

        public override IReadOnlyDictionary<string, string> SortFields => AllowedSortFields;
        public List<TopupTargetStatus>? Statuses { get; set; }
        public string? AccountNumber { get; set; }
    }
}
