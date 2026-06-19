using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.TopUp.Requests;
using Interfaces.TopUp;


namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.FinanceAdmin)]
    public class TopupManagementController(ITopupService topupService) : BaseController
    {
        private readonly ITopupService _topupService = topupService;

        // ─────────────────────────────────────────────────────────────
        // AC3 & AC4: Validation account list Result
        // POST /topup/manual/validate   (multipart/form-data, field: file)
        // ─────────────────────────────────────────────────────────────
        [HttpPost("manual/validate")]
        public async Task<IActionResult> ValidateAccountList(
            IFormFile file,
            CancellationToken cancellationToken)
        {
            var result = await _topupService.ValidateAccountTopupImportAsync(file, cancellationToken);
            return Result.SuccessData(result, "validation completed.");
        }

        // ─────────────────────────────────────────────────────────────
        // AC9 handled client-side (Cancel = do nothing / navigate away)
        // ─────────────────────────────────────────────────────────────

        // ─────────────────────────────────────────────────────────────
        // AC10–AC13: Execute Specific Top-Up (Single & Adhoc)
        // POST /topup/manual/execute
        // ─────────────────────────────────────────────────────────────
        [HttpPost("manual/execute")]
        public async Task<IActionResult> ExecuteManualTopup(
            ManualTopupRequestDTO request,
            CancellationToken cancellationToken)
        {
            var result = await _topupService.ExecuteManualTopupAsync(request, cancellationToken);
            return Result.SuccessData(result, "Manual top-up executed.");
        }
    }
}
