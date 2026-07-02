namespace DTOs.Dashboards
{
    public sealed class DashboardMetricDTO
    {
        public string Label { get; set; } = string.Empty;

        public int Count { get; set; }

        public string? Status { get; set; }
    }

    public sealed class DashboardTrendPointDTO
    {
        public DateOnly Date { get; set; }

        public int Count { get; set; }

        public decimal Amount { get; set; }
    }

    public sealed class SystemDashboardLifecycleTrendPointDTO
    {
        public DateOnly Date { get; set; }

        public int CreatedCount { get; set; }

        public int ClosedCount { get; set; }

        public int ExtendedCount { get; set; }

        public int FailedCount { get; set; }
    }

    public sealed class SystemAdminDashboardDTO
    {
        public int ActiveAccountCount { get; set; }

        public int AccountCreatedTodayCount { get; set; }

        public int ActiveSchoolCount { get; set; }

        public int OpenFailedRecordCount { get; set; }

        public int PendingDailyCreationCount { get; set; }

        public DateTime NextDailyCreationRunAt { get; set; }

        public List<SystemDashboardLifecycleTrendPointDTO> AccountLifecycleTrend { get; set; } = [];

        public List<SystemDashboardReportDTO> LatestReports { get; set; } = [];

        public List<DashboardMetricDTO> ExceptionSummary { get; set; } = [];
    }

    public sealed class SystemDashboardReportDTO
    {
        public DateOnly BatchDate { get; set; }

        public int CreatedCount { get; set; }

        public int ClosedCount { get; set; }

        public int ExtendedCount { get; set; }

        public int FailedCount { get; set; }

        public int PendingCount { get; set; }
    }

    public sealed class FinanceAdminDashboardDTO
    {
        public decimal ExecutedAmount { get; set; }

        public int SuccessfulTargetCount { get; set; }

        public int FailedTargetCount { get; set; }

        public decimal SuccessRate { get; set; }

        public List<DashboardTrendPointDTO> ExecutionTrend { get; set; } = [];

        public List<DashboardMetricDTO> SourceMix { get; set; } = [];

        public List<FinanceDashboardExecutionDTO> RecentExecutions { get; set; } = [];

        public List<FinanceDashboardScheduleDTO> UpcomingSchedules { get; set; } = [];
    }

    public sealed class FinanceDashboardExecutionDTO
    {
        public int Id { get; set; }

        public string ExecutionCode { get; set; } = string.Empty;

        public string? TopupName { get; set; }

        public string SourceType { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public int SuccessCount { get; set; }

        public int FailedCount { get; set; }

        public decimal TotalExecutedAmount { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public sealed class FinanceDashboardScheduleDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal? TopupAmount { get; set; }

        public string Frequency { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime? NextExecutionAt { get; set; }
    }

    public sealed class SchoolAdminDashboardDTO
    {
        public string SchoolName { get; set; } = string.Empty;

        public int ActiveStudentCount { get; set; }

        public int ActiveCourseCount { get; set; }

        public int PendingFasApplicationCount { get; set; }

        public List<DashboardMetricDTO> CourseStatusDistribution { get; set; } = [];

        public List<DashboardMetricDTO> FasStatusSummary { get; set; } = [];

        public List<SchoolDashboardCourseDTO> UpcomingCourses { get; set; } = [];

        public List<SchoolDashboardFasApplicationDTO> PendingFasQueue { get; set; } = [];
    }

    public sealed class SchoolDashboardCourseDTO
    {
        public int Id { get; set; }

        public string CourseCode { get; set; } = string.Empty;

        public string CourseName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public int EnrollmentCount { get; set; }
    }

    public sealed class SchoolDashboardFasApplicationDTO
    {
        public int Id { get; set; }

        public string ApplicationNumber { get; set; } = string.Empty;

        public string AccountName { get; set; } = string.Empty;

        public string SchemeName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime SubmittedAt { get; set; }
    }
}
