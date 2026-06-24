

namespace DTOs.Payments
{
    public class StripeSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string WebhookSecret { get; set; } = string.Empty;
        public string SuccessUrl { get; set; } = string.Empty;
        public string CancelUrl { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public int SessionExpiryMinutes { get; set; }
        public string ClientUrl { get; set; } = string.Empty;
    }

    public class StripeSessionMetadataDTO
    {
        public int AccountId { get; set; }
        public int PaymentId { get; set; }
        public List<int> ChargeIds { get; set; } = [];
        public decimal RemainingPaymentViaStripe { get; set; }
        public Dictionary<int, decimal> ChargeCoveredByCreditBalanceDict { get; set; } = [];
    }

    public class CreatePaymentSessionRequest
    {
        public List<int> CourseIds { get; set; } = [];
    }

    public class PaymentSessionResponseDTO
    {
        public string Link { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
