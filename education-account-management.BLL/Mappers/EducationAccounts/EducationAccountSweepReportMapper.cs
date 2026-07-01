using DTOs.EducationAccounts;
using Models;
using Riok.Mapperly.Abstractions;

namespace Mappers.EducationAccounts
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public static partial class EducationAccountSweepReportMapper
    {
        public static partial EducationAccountSweepResultDTO MapToResultDTO(EducationAccountSweepReport report);

        public static EducationAccountSweepReportSummaryDTO MapToEmptySingleSummary(DateOnly batchDate)
        {
            return new EducationAccountSweepReportSummaryDTO
            {
                BatchDate = batchDate.ToString("yyyy-MM-dd"),
                TotalDuration = "00:00:00",
                AccountsCreatedSuccessfully = 0,
                AccountsFailedManualHandling = 0,
                AccountsClosed = 0,
                AccountsExtended = 0
            };
        }

        public static EducationAccountSweepReportSummaryDTO MapToSingleSummary(
            EducationAccountSweepReport report,
            int failedCount)
        {
            var dailyReport = MapToDailyReport(report, failedCount);
            return new EducationAccountSweepReportSummaryDTO
            {
                BatchDate = dailyReport.BatchDate,
                TotalDuration = dailyReport.TotalDuration,
                AccountsCreatedSuccessfully = dailyReport.AccountsCreatedSuccessfully,
                AccountsFailedManualHandling = dailyReport.AccountsFailedManualHandling,
                AccountsClosed = dailyReport.AccountsClosed,
                AccountsExtended = dailyReport.AccountsExtended
            };
        }

        public static EducationAccountSweepReportSummaryDTO MapToRangeSummary(
            List<EducationAccountSweepReport> reports,
            IReadOnlyDictionary<int, int> failedCounts,
            DateOnly? dateFrom,
            DateOnly? dateTo)
        {
            var dailyReports = reports
                .Select(report => MapToDailyReport(report, failedCounts.GetValueOrDefault(report.Id)))
                .ToList();

            var totalDuration = reports.Aggregate(
                TimeSpan.Zero,
                (total, report) => total + (report.CompletedAt - report.StartedAt));

            return new EducationAccountSweepReportSummaryDTO
            {
                DateFrom = dateFrom?.ToString("yyyy-MM-dd"),
                DateTo = dateTo?.ToString("yyyy-MM-dd"),
                TotalDuration = FormatDuration(totalDuration),
                AccountsCreatedSuccessfully = dailyReports.Sum(report => report.AccountsCreatedSuccessfully),
                AccountsFailedManualHandling = dailyReports.Sum(report => report.AccountsFailedManualHandling),
                AccountsClosed = dailyReports.Sum(report => report.AccountsClosed),
                AccountsExtended = dailyReports.Sum(report => report.AccountsExtended),
                Reports = dailyReports
            };
        }

        private static EducationAccountSweepDailyReportDTO MapToDailyReport(
            EducationAccountSweepReport report,
            int failedCount)
        {
            return new EducationAccountSweepDailyReportDTO
            {
                BatchDate = report.BatchDate.ToString("yyyy-MM-dd"),
                TotalDuration = FormatDuration(report.CompletedAt - report.StartedAt),
                AccountsCreatedSuccessfully = report.AccountsCreatedCount,
                AccountsFailedManualHandling = failedCount,
                AccountsClosed = report.AccountsClosedCount,
                AccountsExtended = report.AccountsExtendedCount
            };
        }

        private static string FormatDuration(TimeSpan duration)
        {
            return $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}";
        }
    }
}
