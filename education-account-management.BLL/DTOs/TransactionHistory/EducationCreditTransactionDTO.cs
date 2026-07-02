using Enums;

namespace DTOs.TransactionHistory
{
    public class EducationCreditTransactionDTO
    {
        public Guid TransactionCode { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Direction { get; set; } = string.Empty;

        public PaymentMethod? PaymentMethod { get; set; }

        public decimal Amount { get; set; }

        public decimal BalanceBefore { get; set; }

        public decimal BalanceAfter { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public EducationCreditTransactionReceiptDTO? Receipt { get; set; }
    }

    public class EducationCreditTransactionReceiptDTO
    {
        public string PaymentMethod { get; set; } = string.Empty;

        public string AccountNumber { get; set; } = string.Empty;

        public string CitizenNric { get; set; } = string.Empty;

        public string CitizenFullName { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        public DateTime? PaidAt { get; set; }

        public string? ExternalReference { get; set; }

        public List<EducationCreditTransactionReceiptItemDTO> Items { get; set; } = [];
    }

    public class EducationCreditTransactionReceiptItemDTO
    {
        public string CourseName { get; set; } = string.Empty;

        public string SchoolName { get; set; } = string.Empty;

        public int? InstallmentNumber { get; set; }

        public decimal Amount { get; set; }
    }
}
