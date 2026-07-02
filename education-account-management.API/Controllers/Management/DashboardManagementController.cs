using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.Dashboards;
using Interfaces.Dashboards;

namespace Controllers.Management
{
    public class DashboardManagementController(
        IDashboardManagementService dashboardManagementService) : BaseController
    {
        private readonly IDashboardManagementService _dashboardManagementService =
            dashboardManagementService;

        [HttpGet("system-admin")]
        [Authorize(Roles = RolePolicy.SystemAdmin)]
        public async Task<IActionResult> GetSystemAdminDashboard(
            [FromQuery] DashboardDateRangeDTO query,
            CancellationToken cancellationToken)
        {
            var result = await _dashboardManagementService.GetSystemAdminDashboardAsync(
                query,
                cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("finance-admin")]
        [Authorize(Roles = RolePolicy.FinanceAdmin)]
        public async Task<IActionResult> GetFinanceAdminDashboard(
            [FromQuery] DashboardDateRangeDTO query,
            CancellationToken cancellationToken)
        {
            var result = await _dashboardManagementService.GetFinanceAdminDashboardAsync(
                query,
                cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("school-admin")]
        [Authorize(Roles = RolePolicy.SchoolAdmin)]
        public async Task<IActionResult> GetSchoolAdminDashboard(
            [FromQuery] DashboardDateRangeDTO query,
            CancellationToken cancellationToken)
        {
            var result = await _dashboardManagementService.GetSchoolAdminDashboardAsync(
                query,
                cancellationToken);
            return Result.SuccessData(result);
        }
    }
}
