namespace DTOs.TransactionHistory
{
    public class EducationCreditTransactionDTO
    {
        public Guid TransactionCode { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Direction { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public decimal BalanceBefore { get; set; }

        public decimal BalanceAfter { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}