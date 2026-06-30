namespace DTOs.Payments
{

    public class StripeSessionMetadataDTO
    {
        public int AccountId { get; set; }
        public List<int> PaymentIds { get; set; } = [];
    }

    public class PaymentRequest
    {
        public List<ChargePaymentRequestInfor> ChargePaymentRequestInfors { get; set; } = [];
        [NumberPositive]
        public decimal CreditBalanceApplied { get; set; } = 0m;
    }

    public class ChargePaymentRequestInfor
    {
        public int ChargeId { get; set; }
        [EnumDefined]
        public PaymentIntent Intent { get; set; }
        [PaymentPlanMonths]
        public int? PaymentPlanMonths { get; set; }
    }
    public class PaymentSessionResponseDTO
    {
        public string Link { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}