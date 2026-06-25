namespace DTOs.TopUp
{
    public sealed class TopupEligibleAccountDTO
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string Nric { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }

    public sealed class TopupExecutionDTO
    {
        public int Id { get; set; }
        public string ExecutionCode { get; set; } = string.Empty;
        public string SourceType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int? SystemTopupId { get; set; }
        public int? ScheduleTopUpId { get; set; }
        public decimal? ManualAmount { get; set; }
        public string? ManualReason { get; set; }
        public int TotalTargetCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public decimal TotalExecutedAmount { get; set; }
        public string? TopupNameSnapshot { get; set; }
        public decimal? TopupAmountSnapshot { get; set; }
        public string? ConditionsSnapshot { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public sealed class TopupExecutionTargetDTO
    {
        public int Id { get; set; }
        public int? EducationAccountId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? FailureReason { get; set; }
        public Guid? TransactionCode { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}