using Authorization;
using Common.HttpResults;
using Controllers.Base;
using Interfaces.EducationAccounts;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SystemAdmin)]
    public class EducationAccountSweepReportManagementController(IEducationAccountSweepReportService batchReportService) : BaseController
    {
        private readonly IEducationAccountSweepReportService _batchReportService = batchReportService;

        [HttpGet]
        public async Task<IActionResult> GetReport(
            [FromQuery] DateOnly? date,
            CancellationToken cancellationToken)
        {
            var result = await _batchReportService.GetReportAsync(date, cancellationToken);
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