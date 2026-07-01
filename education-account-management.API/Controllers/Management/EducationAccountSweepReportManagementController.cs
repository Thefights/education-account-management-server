using Authorization;
using Common.HttpResults;
using Controllers.Base;
using Filters.EducationAccounts;
using Interfaces.EducationAccounts;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SystemAdmin)]
    public class EducationAccountSweepReportManagementController(IEducationAccountSweepReportService batchReportService) : BaseController
    {
        private readonly IEducationAccountSweepReportService _batchReportService = batchReportService;

        [HttpGet]
        public async Task<IActionResult> GetReport(
            [FromQuery] EducationAccountSweepReportQueryDTO query,
            CancellationToken cancellationToken)
        {
            var result = await _batchReportService.GetReportAsync(query, cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("targets")]
        public async Task<IActionResult> GetReportTargets(
            [FromQuery] EducationAccountSweepTargetRangeFilterDTO filter,
            CancellationToken cancellationToken)
        {
            var result = await _batchReportService.GetReportTargetsAsync(filter, cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("{batchDate}/targets")]
        public async Task<IActionResult> GetReportTargets(
            [FromRoute] DateOnly batchDate,
            [FromQuery] EducationAccountSweepTargetFilterDTO filter,
            CancellationToken cancellationToken)
        {
            var result = await _batchReportService.GetReportTargetsAsync(batchDate, filter, cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("failed-records/manual-handling")]
        public async Task<IActionResult> GetFailedRecordForManualHandling(
            [FromQuery] string nric,
            [FromQuery] DateOnly batchRunDate,
            CancellationToken cancellationToken)
        {
            var result = await _batchReportService.GetFailedRecordForManualHandlingAsync(
                nric,
                batchRunDate,
                cancellationToken);

            return Result.SuccessData(result);
        }
    }
}
