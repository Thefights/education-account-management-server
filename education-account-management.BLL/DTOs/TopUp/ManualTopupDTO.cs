using System.ComponentModel.DataAnnotations;

namespace DTOs.TopUp
{
    // ── CSV import row ──────────────────────────────────────────────────────

    public class ManualTopupImportRowDTO
    {
        public string? AccountNumber { get; set; }
    }

    // ── Result item base ────────────────────────────────────────────────────

    public class ManualTopupItem
    {
        public int AccountId { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public string AccountName { get; set; } = string.Empty;

        public decimal TopUpAmount { get; set; }
    }

    // ── Result items ────────────────────────────────────────────────────────

    public class TopupSuccessItemDTO : ManualTopupItem
    {
        public Guid TopUpTransactionId { get; set; }
    }

    public class TopupFailItemDTO : ManualTopupItem
    {
        public string Reason { get; set; } = string.Empty;
    }

    // ── Execution result ────────────────────────────────────────────────────

    public class ExecuteTopupResultDTO
    {
        public int BatchId { get; set; }

        public int TotalProcessed { get; set; }

        public int TotalSuccess { get; set; }

        public int TotalFailed { get; set; }

        public decimal TotalAmountCredited { get; set; }

        public List<TopupSuccessItemDTO> SuccessList { get; set; } = [];

        public List<TopupFailItemDTO> FailList { get; set; } = [];
    }

    // ── Execute request ─────────────────────────────────────────────────────

    public class ManualTopupRequestDTO
    {
        [MessageRequired]
        [NumberHigherThan(0)]
        public decimal TopUpAmount { get; set; }

        [MessageRequired]
        [MessageMaxLength(500)]
        public string DisbursementReason { get; set; } = string.Empty;

        [MessageRequired]
        [MessageMaxLength(100)]
        public string IdempotencyKey { get; set; } = string.Empty;

        /// <summary>
        /// Used when the operator selects accounts from the UI table.
        /// Mutually exclusive with <see cref="File"/>.
        /// </summary>
        public List<int>? AccountIds { get; set; }

        /// <summary>
        /// CSV file with AccountNumber column.
        /// Mutually exclusive with <see cref="AccountIds"/>.
        /// </summary>
        public IFormFile? File { get; set; }

    }
}
