using System.ComponentModel.DataAnnotations;

namespace DTOs.Payments
{

    public class StripeSessionMetadataDTO
    {
        public int AccountId { get; set; }
        public List<StripeSessionBillingActionDTO> BillingActions { get; set; } = [];
    }

    public class StripeSessionBillingActionDTO
    {
        public int ChargeId { get; set; }
        public PaymentIntent Intent { get; set; }
        public int? PaymentPlanMonths { get; set; }
        public int? InstallmentCount { get; set; }
    }

    public class PayFullChargesRequest
    {
        [MessageMinLength(1)]
        public List<int> ChargeIds { get; set; } = [];

        [NumberPositive]
        public decimal CreditBalanceApplied { get; set; } = 0m;
    }

    public class CreateInstallmentPlansRequest
    {
        [MessageMinLength(1)]
        public List<CreateInstallmentPlanItemRequest> Items { get; set; } = [];

        [NumberPositive]
        public decimal CreditBalanceApplied { get; set; } = 0m;
    }

    public class CreateInstallmentPlanItemRequest
    {
        public int ChargeId { get; set; }

        [PaymentPlanMonths]
        public int PaymentPlanMonths { get; set; }
    }

    public class PayDueInstallmentsRequest
    {
        [MessageMinLength(1)]
        public List<PayDueInstallmentsItemRequest> Items { get; set; } = [];

        [NumberPositive]
        public decimal CreditBalanceApplied { get; set; } = 0m;
    }

    public class PayDueInstallmentsItemRequest
    {
        public int ChargeId { get; set; }

        [Range(1, int.MaxValue)]
        public int InstallmentCount { get; set; }
    }

    public class PayRemainingInstallmentsRequest
    {
        [MessageMinLength(1)]
        public List<int> ChargeIds { get; set; } = [];

        [NumberPositive]
        public decimal CreditBalanceApplied { get; set; } = 0m;
    }

    public class PaymentSessionResponseDTO
    {
        public string Link { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PaymentMode { get; set; } = string.Empty;
        public List<int> PaymentIds { get; set; } = [];
        public decimal TotalAmount { get; set; }
        public decimal WalletAmount { get; set; }
        public decimal OnlineAmount { get; set; }
        public bool IsWalletOnly { get; set; }
        public bool RequiresRedirect { get; set; }
    }
}
