using Stripe;
using Stripe.Checkout;

namespace Interfaces.Payments
{
    public interface IStripeCheckoutGateway
    {
        Task<Session> CreateAsync(SessionCreateOptions options, CancellationToken cancellationToken = default);

        Task<Session> GetAsync(string sessionId, CancellationToken cancellationToken = default);

        Task<Session> ExpireAsync(string sessionId, CancellationToken cancellationToken = default);

        Event ConstructEvent(string payload, string signature, string webhookSecret);
    }
}
