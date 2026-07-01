using DTOs.Payments;

namespace Interfaces.Payments
{
    public interface IStripeService
    {
        Task<PaymentSessionResponseDTO> PayFullChargesAsync(PayFullChargesRequest request, CancellationToken token);
        Task<PaymentSessionResponseDTO> CreateInstallmentPlansAsync(CreateInstallmentPlansRequest request, CancellationToken token);
        Task<PaymentSessionResponseDTO> PayDueInstallmentsAsync(PayDueInstallmentsRequest request, CancellationToken token);
        Task<PaymentSessionResponseDTO> PayRemainingInstallmentsAsync(PayRemainingInstallmentsRequest request, CancellationToken token);
        Task HandleWebhookAsync(string payload, string stripeSignature);
        Task<PaymentSessionResponseDTO> HandleSessionCancelledAsync(string sessionId, CancellationToken token);
        Task<PaymentSessionResponseDTO> HandleSessionSuccessAsync(string sessionId, CancellationToken token);
    }
}
