
using EntityAnnotations;

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
        public decimal CreditBalanceApplied { get; set; } = 0m;
    }

    public enum PaymentIntent
    {
        PayFull = 1,                 // Trả thẳng toàn bộ (Không trả góp)
        CreateInstallment = 2,       // Tạo mới trả góp (Dùng kèm InstallmentNumber)
        PayCurrentInstallment = 3,   // Trả tháng hiện tại của trả góp đang có
        PayRemainingInstallments = 4 // Tất toán toàn bộ số tháng trả góp còn nợ
    }

    public class ChargePaymentRequestInfor
    {
        public int ChargeId { get; set; }
        public PaymentIntent Intent { get; set; }
        [PaymentPlanMonths]
        public int? InstallmentNumber { get; set; }
    }
    public class PaymentSessionResponseDTO
    {
        public string Link { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}