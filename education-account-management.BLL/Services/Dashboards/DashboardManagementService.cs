using DTOs.Dashboards;
using Interfaces.Dashboards;

namespace Services.Dashboards
{
    public sealed class DashboardManagementService(
        IUnitOfWork unitOfWork,
        SchoolScopeResolver schoolScopeResolver) : IDashboardManagementService
    {
        private readonly IGenericRepository<EducationAccount> _educationAccountRepository =
            unitOfWork.Repository<EducationAccount>();
        private readonly IGenericRepository<School> _schoolRepository =
            unitOfWork.Repository<School>();
        private readonly IGenericRepository<EducationAccountSweepReport> _sweepReportRepository =
            unitOfWork.Repository<EducationAccountSweepReport>();
        private readonly IGenericRepository<EducationAccountSweepTarget> _sweepTargetRepository =
            unitOfWork.Repository<EducationAccountSweepTarget>();
        private readonly IGenericRepository<TopupExecution> _topupExecutionRepository =
            unitOfWork.Repository<TopupExecution>();
        private readonly IGenericRepository<TopupExecutionTarget> _topupExecutionTargetRepository =
            unitOfWork.Repository<TopupExecutionTarget>();
        private readonly IGenericRepository<ScheduleTopUp> _scheduleTopUpRepository =
            unitOfWork.Repository<ScheduleTopUp>();
        private readonly IGenericRepository<SchoolStudent> _schoolStudentRepository =
            unitOfWork.Repository<SchoolStudent>();
        private readonly IGenericRepository<Course> _courseRepository =
            unitOfWork.Repository<Course>();
        private readonly IGenericRepository<FasApplication> _fasApplicationRepository =
            unitOfWork.Repository<FasApplication>();
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;

        public async Task<SystemAdminDashboardDTO> GetSystemAdminDashboardAsync(
            DashboardDateRangeDTO query,
            CancellationToken cancellationToken = default)
        {
            var range = ResolveRange(query, DashboardRangeDefault.LastThirtySingaporeDays);
            var singaporeToday = DateOnly.FromDateTime(ToSingapore(DateTime.UtcNow));
            var reportQuery = _sweepReportRepository.Query()
                .Where(report => report.BatchDate >= range.From && report.BatchDate <= range.To);

            var reports = await reportQuery
                .OrderBy(report => report.BatchDate)
                .Select(report => new SystemDashboardReportDTO
                {
                    BatchDate = report.BatchDate,
                    CreatedCount = report.AccountsCreatedCount,
                    ClosedCount = report.AccountsClosedCount,
                    ExtendedCount = report.AccountsExtendedCount,
                    FailedCount = report.Targets.Count(target => target.Status == SweepTargetStatus.Failed),
                    PendingCount = report.Targets.Count(target => target.Status == SweepTargetStatus.Pending)
                })
                .ToListAsync(cancellationToken);

            var latestReports = reports
                .OrderByDescending(report => report.BatchDate)
                .Take(5)
                .ToList();

            var failedRecordCount = await _sweepTargetRepository.CountAsync(
                target => target.Status == SweepTargetStatus.Failed,
                cancellationToken);
            var pendingDailyCreationCount = await _sweepTargetRepository.CountAsync(
                target => target.Status == SweepTargetStatus.Pending &&
                    target.Action == SweepAction.Create,
                cancellationToken);

            return new SystemAdminDashboardDTO
            {
                ActiveAccountCount = await _educationAccountRepository.CountAsync(
                    account => account.Status == EducationAccountStatus.Active,
                    cancellationToken),
                AccountCreatedTodayCount = reports
                    .Where(report => report.BatchDate == singaporeToday)
                    .Sum(report => report.CreatedCount),
                ActiveSchoolCount = await _schoolRepository.CountAsync(
                    school => school.Status == SchoolStatus.Active,
                    cancellationToken),
                OpenFailedRecordCount = failedRecordCount,
                PendingDailyCreationCount = pendingDailyCreationCount,
                NextDailyCreationRunAt = GetNextSingaporeMidnightUtc(),
                AccountLifecycleTrend = BuildSystemLifecycleTrend(reports, range.From, range.To),
                LatestReports = latestReports,
                ExceptionSummary =
                [
                    new()
                    {
                        Label = "Failed account records",
                        Count = failedRecordCount,
                        Status = SweepTargetStatus.Failed.ToString()
                    },
                    new()
                    {
                        Label = "Daily account creation",
                        Count = pendingDailyCreationCount,
                        Status = SweepTargetStatus.Pending.ToString()
                    },
                    new()
                    {
                        Label = "Successful targets",
                        Count = await _sweepTargetRepository.CountAsync(
                            target => target.Status == SweepTargetStatus.Success,
                            cancellationToken),
                        Status = SweepTargetStatus.Success.ToString()
                    }
                ]
            };
        }

        public async Task<FinanceAdminDashboardDTO> GetFinanceAdminDashboardAsync(
            DashboardDateRangeDTO query,
            CancellationToken cancellationToken = default)
        {
            var range = ResolveRange(query, DashboardRangeDefault.CurrentSingaporeMonth);
            var (fromUtc, toUtcExclusive) = ToUtcRange(range);

            var executions = await _topupExecutionRepository.Query()
                .Where(execution => execution.CreatedAt >= fromUtc && execution.CreatedAt < toUtcExclusive)
                .OrderByDescending(execution => execution.CreatedAt)
                .Select(execution => new FinanceDashboardExecutionDTO
                {
                    Id = execution.Id,
                    ExecutionCode = execution.ExecutionCode,
                    TopupName = execution.TopupNameSnapshot,
                    SourceType = execution.SourceType.ToString(),
                    Status = execution.Status.ToString(),
                    SuccessCount = execution.SuccessCount,
                    FailedCount = execution.FailedCount,
                    TotalExecutedAmount = execution.TotalExecutedAmount,
                    CreatedAt = execution.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var executionIds = executions.Select(execution => execution.Id).ToList();
            var targetCounts = executionIds.Count == 0
                ? new List<TargetStatusCount>()
                : await _topupExecutionTargetRepository.Query()
                    .Where(target => executionIds.Contains(target.TopupExecutionId))
                    .GroupBy(target => target.Status)
                    .Select(group => new TargetStatusCount
                    {
                        Status = group.Key,
                        Count = group.Count()
                    })
                    .ToListAsync(cancellationToken);

            var successfulTargetCount = targetCounts
                .Where(item => item.Status == TopupTargetStatus.Success)
                .Sum(item => item.Count);
            var failedTargetCount = targetCounts
                .Where(item => item.Status == TopupTargetStatus.Failed)
                .Sum(item => item.Count);
            var totalTargetCount = targetCounts.Sum(item => item.Count);

            var upcomingSchedules = await _scheduleTopUpRepository.Query()
                .Where(schedule => schedule.Status == ScheduleTopUpStatus.Active &&
                    schedule.NextExecutionAt.HasValue &&
                    schedule.NextExecutionAt.Value >= DateTime.UtcNow)
                .OrderBy(schedule => schedule.NextExecutionAt)
                .Take(5)
                .Select(schedule => new FinanceDashboardScheduleDTO
                {
                    Id = schedule.Id,
                    Name = schedule.Name,
                    TopupAmount = schedule.TopupAmount,
                    Frequency = schedule.Frequency.ToString(),
                    Status = schedule.Status.ToString(),
                    NextExecutionAt = schedule.NextExecutionAt
                })
                .ToListAsync(cancellationToken);

            return new FinanceAdminDashboardDTO
            {
                ExecutedAmount = executions.Sum(execution => execution.TotalExecutedAmount),
                SuccessfulTargetCount = successfulTargetCount,
                FailedTargetCount = failedTargetCount,
                SuccessRate = totalTargetCount == 0
                    ? 0
                    : Math.Round(successfulTargetCount * 100m / totalTargetCount, 2),
                ExecutionTrend = BuildExecutionTrend(executions, range.From, range.To),
                SourceMix = executions
                    .GroupBy(execution => execution.SourceType)
                    .Select(group => new DashboardMetricDTO
                    {
                        Label = group.Key,
                        Count = group.Count(),
                        Status = group.Key
                    })
                    .OrderByDescending(item => item.Count)
                    .ToList(),
                RecentExecutions = executions.Take(5).ToList(),
                UpcomingSchedules = upcomingSchedules
            };
        }

        public async Task<SchoolAdminDashboardDTO> GetSchoolAdminDashboardAsync(
            DashboardDateRangeDTO query,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var range = ResolveRange(query, DashboardRangeDefault.CurrentSingaporeMonth);
            var (fromUtc, toUtcExclusive) = ToUtcRange(range);

            var schoolName = await _schoolRepository.Query()
                .Where(school => school.Id == schoolId)
                .Select(school => school.SchoolName)
                .SingleOrDefaultAsync(cancellationToken) ?? string.Empty;

            var courseStatusCounts = await _courseRepository.Query()
                .Where(course => course.SchoolId == schoolId)
                .GroupBy(course => course.Status)
                .Select(group => new CourseStatusCount
                {
                    Status = group.Key,
                    Count = group.Count()
                })
                .ToListAsync(cancellationToken);

            var fasStatusCounts = await _fasApplicationRepository.Query()
                .Where(application => application.SchoolStudent.SchoolId == schoolId &&
                    application.CreatedAt >= fromUtc &&
                    application.CreatedAt < toUtcExclusive)
                .GroupBy(application => application.Status)
                .Select(group => new FasStatusCount
                {
                    Status = group.Key,
                    Count = group.Count()
                })
                .ToListAsync(cancellationToken);

            var upcomingCourses = await _courseRepository.Query()
                .Where(course => course.SchoolId == schoolId &&
                    course.Status == CourseStatus.Upcoming)
                .OrderBy(course => course.StartDate)
                .Take(5)
                .Select(course => new SchoolDashboardCourseDTO
                {
                    Id = course.Id,
                    CourseCode = course.CourseCode,
                    CourseName = course.CourseName,
                    Status = course.Status.ToString(),
                    StartDate = course.StartDate,
                    EnrollmentCount = course.Enrollments.Count
                })
                .ToListAsync(cancellationToken);

            var pendingFasQueue = await _fasApplicationRepository.Query()
                .Where(application => application.SchoolStudent.SchoolId == schoolId &&
                    application.Status == FasApplicationStatus.Pending)
                .OrderBy(application => application.CreatedAt)
                .Take(5)
                .Select(application => new SchoolDashboardFasApplicationDTO
                {
                    Id = application.Id,
                    ApplicationNumber = application.ApplicationNumber,
                    AccountName = application.SchoolStudent.EducationAccount.Citizen.FullName,
                    SchemeName = application.FasScheme.SchemeName,
                    Status = application.Status.ToString(),
                    SubmittedAt = application.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new SchoolAdminDashboardDTO
            {
                SchoolName = schoolName,
                ActiveStudentCount = await _schoolStudentRepository.CountAsync(
                    student => student.SchoolId == schoolId &&
                        student.Status == SchoolStudentStatus.Active,
                    cancellationToken),
                ActiveCourseCount = courseStatusCounts
                    .Where(item => item.Status is CourseStatus.Enrolling or CourseStatus.InProgress)
                    .Sum(item => item.Count),
                PendingFasApplicationCount = await _fasApplicationRepository.CountAsync(
                    application => application.SchoolStudent.SchoolId == schoolId &&
                        application.Status == FasApplicationStatus.Pending,
                    cancellationToken),
                CourseStatusDistribution =
                [
                    new()
                    {
                        Label = "Active",
                        Count = courseStatusCounts
                            .Where(item => item.Status is CourseStatus.Enrolling or CourseStatus.InProgress)
                            .Sum(item => item.Count),
                        Status = "Active"
                    },
                    new()
                    {
                        Label = CourseStatus.Upcoming.ToString(),
                        Count = courseStatusCounts
                            .Where(item => item.Status == CourseStatus.Upcoming)
                            .Sum(item => item.Count),
                        Status = CourseStatus.Upcoming.ToString()
                    },
                    new()
                    {
                        Label = CourseStatus.Draft.ToString(),
                        Count = courseStatusCounts
                            .Where(item => item.Status == CourseStatus.Draft)
                            .Sum(item => item.Count),
                        Status = CourseStatus.Draft.ToString()
                    },
                    new()
                    {
                        Label = CourseStatus.Closed.ToString(),
                        Count = courseStatusCounts
                            .Where(item => item.Status == CourseStatus.Closed)
                            .Sum(item => item.Count),
                        Status = CourseStatus.Closed.ToString()
                    }
                ],
                FasStatusSummary =
                [
                    BuildFasStatusMetric(fasStatusCounts, FasApplicationStatus.Pending),
                    BuildFasStatusMetric(fasStatusCounts, FasApplicationStatus.Approved),
                    BuildFasStatusMetric(fasStatusCounts, FasApplicationStatus.Rejected),
                    BuildFasStatusMetric(fasStatusCounts, FasApplicationStatus.Expired)
                ],
                UpcomingCourses = upcomingCourses,
                PendingFasQueue = pendingFasQueue
            };
        }

        private static DashboardDateRange ResolveRange(
            DashboardDateRangeDTO query,
            DashboardRangeDefault rangeDefault)
        {
            var singaporeToday = DateOnly.FromDateTime(ToSingapore(DateTime.UtcNow));
            var from = query.DateFrom;
            var to = query.DateTo;

            if (!from.HasValue || !to.HasValue)
            {
                if (query.RangeDays is > 0)
                {
                    var rangeDays = Math.Clamp(query.RangeDays.Value, 1, 90);
                    (from, to) = (singaporeToday.AddDays(-(rangeDays - 1)), singaporeToday);
                }
                else
                {
                    (from, to) = rangeDefault == DashboardRangeDefault.CurrentSingaporeMonth
                        ? (new DateOnly(singaporeToday.Year, singaporeToday.Month, 1), singaporeToday)
                        : (singaporeToday.AddDays(rangeDefault == DashboardRangeDefault.LastThirtySingaporeDays
                        ? -29
                        : -6), singaporeToday);
                }
            }

            if (from.Value > to.Value)
            {
                (from, to) = (to, from);
            }

            return new DashboardDateRange(from.Value, to.Value);
        }

        private static (DateTime FromUtc, DateTime ToUtcExclusive) ToUtcRange(
            DashboardDateRange range)
        {
            return (
                ToUtcFromSingaporeDate(range.From),
                ToUtcFromSingaporeDate(range.To.AddDays(1)));
        }

        private static DateTime ToUtcFromSingaporeDate(DateOnly date)
        {
            return date.ToDateTime(TimeOnly.MinValue).AddHours(-8);
        }

        private static DateTime ToSingapore(DateTime utcDateTime)
        {
            return utcDateTime.AddHours(8);
        }

        private static DateTime GetNextSingaporeMidnightUtc()
        {
            var singaporeNow = ToSingapore(DateTime.UtcNow);
            return singaporeNow.Date.AddDays(1).AddHours(-8);
        }

        private static List<DashboardTrendPointDTO> BuildExecutionTrend(
            List<FinanceDashboardExecutionDTO> executions,
            DateOnly from,
            DateOnly to)
        {
            var grouped = executions
                .GroupBy(execution => DateOnly.FromDateTime(ToSingapore(execution.CreatedAt)))
                .ToDictionary(
                    group => group.Key,
                    group => new
                    {
                        Count = group.Sum(execution => execution.FailedCount),
                        Amount = group.Sum(execution => execution.TotalExecutedAmount)
                    });

            var result = new List<DashboardTrendPointDTO>();
            for (var date = from; date <= to; date = date.AddDays(1))
            {
                grouped.TryGetValue(date, out var value);
                result.Add(new DashboardTrendPointDTO
                {
                    Date = date,
                    Count = value?.Count ?? 0,
                    Amount = value?.Amount ?? 0
                });
            }

            return result;
        }

        private static List<SystemDashboardLifecycleTrendPointDTO> BuildSystemLifecycleTrend(
            List<SystemDashboardReportDTO> reports,
            DateOnly from,
            DateOnly to)
        {
            var reportsByDate = reports
                .GroupBy(report => report.BatchDate)
                .ToDictionary(
                    group => group.Key,
                    group => new
                    {
                        CreatedCount = group.Sum(report => report.CreatedCount),
                        ClosedCount = group.Sum(report => report.ClosedCount),
                        ExtendedCount = group.Sum(report => report.ExtendedCount),
                        FailedCount = group.Sum(report => report.FailedCount)
                    });
            var result = new List<SystemDashboardLifecycleTrendPointDTO>();

            for (var date = from; date <= to; date = date.AddDays(1))
            {
                reportsByDate.TryGetValue(date, out var report);
                result.Add(new SystemDashboardLifecycleTrendPointDTO
                {
                    Date = date,
                    CreatedCount = report?.CreatedCount ?? 0,
                    ClosedCount = report?.ClosedCount ?? 0,
                    ExtendedCount = report?.ExtendedCount ?? 0,
                    FailedCount = report?.FailedCount ?? 0
                });
            }

            return result;
        }

        private static DashboardMetricDTO BuildFasStatusMetric(
            List<FasStatusCount> counts,
            FasApplicationStatus status)
        {
            return new DashboardMetricDTO
            {
                Label = status.ToString(),
                Count = counts
                    .Where(item => item.Status == status)
                    .Sum(item => item.Count),
                Status = status.ToString()
            };
        }

        private sealed record DashboardDateRange(DateOnly From, DateOnly To);

        private enum DashboardRangeDefault
        {
            LastSevenSingaporeDays,
            LastThirtySingaporeDays,
            CurrentSingaporeMonth
        }

        private sealed class TargetStatusCount
        {
            public TopupTargetStatus Status { get; set; }

            public int Count { get; set; }
        }

        private sealed class CourseStatusCount
        {
            public CourseStatus Status { get; set; }

            public int Count { get; set; }
        }

        private sealed class FasStatusCount
        {
            public FasApplicationStatus Status { get; set; }

            public int Count { get; set; }
        }
    }
}
