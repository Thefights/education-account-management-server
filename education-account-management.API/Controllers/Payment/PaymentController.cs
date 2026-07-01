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

        [HttpPost("full")]
        [Authorize(Roles = RolePolicy.AccountHolder)]
        public async Task<IActionResult> PayFull([FromForm] PayFullChargesRequest request, CancellationToken token)
        {
            var response = await _stripeService.PayFullChargesAsync(request, token);
            return Result.SuccessData(response);
        }

        [HttpPost("installment-plan")]
        [Authorize(Roles = RolePolicy.AccountHolder)]
        public async Task<IActionResult> CreateInstallmentPlan([FromForm] CreateInstallmentPlansRequest request, CancellationToken token)
        {
            var response = await _stripeService.CreateInstallmentPlansAsync(request, token);
            return Result.SuccessData(response);
        }

        [HttpPost("installments/next")]
        [Authorize(Roles = RolePolicy.AccountHolder)]
        public async Task<IActionResult> PayNextInstallments([FromForm] PayNextInstallmentsRequest request, CancellationToken token)
        {
            var response = await _stripeService.PayNextInstallmentsAsync(request, token);
            return Result.SuccessData(response);
        }

        [HttpPost("installments/remaining")]
        [Authorize(Roles = RolePolicy.AccountHolder)]
        public async Task<IActionResult> PayRemainingInstallments([FromForm] PayRemainingInstallmentsRequest request, CancellationToken token)
        {
            var response = await _stripeService.PayRemainingInstallmentsAsync(request, token);
            return Result.SuccessData(response);
        }

        [HttpPost("success")]
        [Authorize(Roles = RolePolicy.AccountHolder)]
        public async Task<IActionResult> Success([FromForm] string sessionId, CancellationToken token)
        {
            var response = await _stripeService.HandleSessionSuccessAsync(sessionId, token);

            return Result.SuccessData(response);
        }

        [HttpPost("cancel")]
        [Authorize(Roles = RolePolicy.AccountHolder)]
        public async Task<IActionResult> Cancel([FromForm] string sessionId, CancellationToken token)
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
