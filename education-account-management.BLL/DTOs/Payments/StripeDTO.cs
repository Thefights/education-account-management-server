

namespace DTOs.Payments
{

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
