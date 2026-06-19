using Authorization;
using Common.HttpResults;
using Controllers.Base;
using Interfaces.BatchReport;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SystemAdmin)]
    public class BatchReportManagementController(IBatchReportService batchReportService) : BaseController
    {
        private readonly IBatchReportService _batchReportService = batchReportService;

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
