using Interfaces.Payments;
using Stripe;
using Stripe.Checkout;

namespace Services.Payments
{
    public class StripeCheckoutGateway(AppConfiguration configuration) : IStripeCheckoutGateway
    {
        private readonly StripeClient _stripeClient = new(configuration.StripeConfig.SecretKey);

        public async Task<Session> CreateAsync(
            SessionCreateOptions options,
            CancellationToken cancellationToken = default)
        {
            var sessionService = new SessionService(_stripeClient);
            return await sessionService.CreateAsync(options, cancellationToken: cancellationToken);
        }

        public async Task<Session> GetAsync(
            string sessionId,
            CancellationToken cancellationToken = default)
        {
            var sessionService = new SessionService(_stripeClient);
            return await sessionService.GetAsync(sessionId, cancellationToken: cancellationToken);
        }

        public async Task<Session> ExpireAsync(
            string sessionId,
            CancellationToken cancellationToken = default)
        {
            var sessionService = new SessionService(_stripeClient);
            return await sessionService.ExpireAsync(sessionId, cancellationToken: cancellationToken);
        }

        public Event ConstructEvent(string payload, string signature, string webhookSecret)
        {
            return EventUtility.ConstructEvent(payload, signature, webhookSecret);
        }
    }
}
