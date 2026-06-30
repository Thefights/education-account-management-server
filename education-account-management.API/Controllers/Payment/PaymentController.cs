using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.Payments;
using Interfaces.Payments;


namespace Controllers.Payment
{
    public class PaymentController(IStripeService stripeService) : BaseController
    {
        private readonly IStripeService _stripeService = stripeService;

        [HttpPost("handle")]
        [Authorize(Roles = RolePolicy.AccountHolder)]
        public async Task<IActionResult> HandlePayment([FromBody] PaymentRequest request, CancellationToken token)
        {
            var response = await _stripeService.HandlePaymentSessionAsync(request, token);
            return Result.SuccessData(response);
        }

        [HttpPost("success")]
        [Authorize(Roles = RolePolicy.AccountHolder)]
        public async Task<IActionResult> Success([FromQuery(Name = "session_id")] string sessionId, CancellationToken token)
        {
            var response = await _stripeService.HandleSessionSuccessAsync(sessionId, token);

            return Result.SuccessData(response);
        }

        [HttpPost("cancel")]
        [Authorize(Roles = RolePolicy.AccountHolder)]
        public async Task<IActionResult> Cancel([FromQuery(Name = "session_id")] string sessionId, CancellationToken token)
        {
            var response = await _stripeService.HandleSessionCancelledAsync(sessionId, token);

            return Result.SuccessData(response);
        }

        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> Webhook()
        {
            var payload = await new StreamReader(Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"];

            await _stripeService.HandleWebhookAsync(payload, signature!);
            return Ok();
        }
    }
}