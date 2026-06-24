using Authorization;
using BLL.Interfaces.Payments;
using Controllers.Base;
using DTOs.Payments;
using Enums;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace API.Controllers.Payment
{
    public class PaymentController(IStripeService stripeService) : BaseController
    {
        private readonly IStripeService _stripeService = stripeService;

        [HttpPost("create-session")]
        [Authorize(Roles = RolePolicy.AccountHolder)]
        public async Task<IActionResult> CreateSession([FromBody] CreatePaymentSessionRequest request, CancellationToken token)
        {
            var response = await _stripeService.CreateCheckoutSessionAsync(request.CourseIds, token);
            return Ok(response);
        }

        [HttpPost("success")]
        [Authorize(Roles = RolePolicy.AccountHolder)]
        public async Task<IActionResult> Success([FromQuery(Name = "session_id")] string sessionId, CancellationToken token)
        {
            var response = await _stripeService.HandleSessionSuccessAsync(sessionId, token);

            return Ok(response);
        }

        [HttpPost("cancel")]
        [Authorize(Roles = RolePolicy.AccountHolder)]
        public async Task<IActionResult> Cancel([FromQuery(Name = "session_id")] string sessionId, CancellationToken token)
        {
            var response = await _stripeService.HandleSessionCancelledAsync(sessionId, token);

            return Ok(response);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var payload = await new StreamReader(Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"];

            await _stripeService.HandleWebhookAsync(payload, signature!);
            return Ok();
        }
    }
}
