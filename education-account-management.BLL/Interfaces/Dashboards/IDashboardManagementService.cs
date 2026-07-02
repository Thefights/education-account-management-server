using DTOs.Dashboards;

namespace Interfaces.Dashboards
{
    public interface IDashboardManagementService
    {
        Task<SystemAdminDashboardDTO> GetSystemAdminDashboardAsync(
            DashboardDateRangeDTO query,
            CancellationToken cancellationToken = default);

        Task<FinanceAdminDashboardDTO> GetFinanceAdminDashboardAsync(
            DashboardDateRangeDTO query,
            CancellationToken cancellationToken = default);

        Task<SchoolAdminDashboardDTO> GetSchoolAdminDashboardAsync(
            DashboardDateRangeDTO query,
            CancellationToken cancellationToken = default);
    }
}
