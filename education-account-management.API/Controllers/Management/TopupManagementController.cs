using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.TopUp;
using Interfaces.TopUp;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.FinanceAdmin)]
    public class TopupManagementController(ITopupService topupService) : BaseController
    {
        private readonly ITopupService _topupService = topupService;

        // POST /topup/manual/execute    (multipart/form-data or JSON)
        [HttpPost("manual/execute")]
        public async Task<IActionResult> ExecuteManualTopup(
            [FromForm] ManualTopupRequestDTO request,
            CancellationToken cancellationToken)
        {
            var result = await _topupService.ExecuteManualTopupAsync(request, cancellationToken);
            return Result.SuccessData(result, "Manual top-up executed.");
        }
    }
}
