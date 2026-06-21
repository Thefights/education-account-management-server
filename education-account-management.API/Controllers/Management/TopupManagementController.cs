using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.TopUp;
using Interfaces.TopUp;
using Filters.TopUp;
using Results;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.FinanceAdmin)]
    public class TopupManagementController(
        ITopupService topupService,
        ITopupManagementQueryService queryService) : BaseController
    {
        private readonly ITopupService _topupService = topupService;
        private readonly ITopupManagementQueryService _queryService = queryService;

        [HttpGet("eligible-accounts")]
        public async Task<IActionResult> GetEligibleAccounts(
            [FromQuery] TopupAccountLookupFilterDTO filter,
            CancellationToken cancellationToken)
        {
            return Result.SuccessData(await _queryService.GetEligibleAccountsAsync(filter, cancellationToken));
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory(
            [FromQuery] TopupExecutionFilterDTO filter,
            CancellationToken cancellationToken)
        {
            return Result.SuccessData(await _queryService.GetHistoryAsync(filter, cancellationToken));
        }

        [HttpGet("history/{id:int}")]
        public async Task<IActionResult> GetHistoryById(int id, CancellationToken cancellationToken)
        {
            return Result.SuccessData(await _queryService.GetHistoryByIdAsync(id, cancellationToken));
        }

        [HttpGet("history/{id:int}/targets")]
        public async Task<IActionResult> GetHistoryTargets(
            int id,
            [FromQuery] TopupExecutionTargetFilterDTO filter,
            CancellationToken cancellationToken)
        {
            return Result.SuccessData(await _queryService.GetTargetsAsync(id, filter, cancellationToken));
        }

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
