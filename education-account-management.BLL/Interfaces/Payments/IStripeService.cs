using DTOs.Payments;

namespace BLL.Interfaces.Payments
{
    public interface IStripeService
    {
        Task<PaymentSessionResponseDTO> CreateCheckoutSessionAsync(List<int> chargeIds, CancellationToken token);
        Task HandleWebhookAsync(string payload, string stripeSignature);
        Task<PaymentSessionResponseDTO> HandleSessionCancelledAsync(string sessionId, CancellationToken token);
        Task<PaymentSessionResponseDTO> HandleSessionSuccessAsync(string sessionId, CancellationToken token);
    }
}
