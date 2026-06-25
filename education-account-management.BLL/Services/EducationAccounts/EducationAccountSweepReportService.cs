using DTOs.EducationAccounts;
using Filters.EducationAccounts;
using Interfaces.EducationAccounts;
using Results;

namespace Services.EducationAccounts
{
    public class EducationAccountSweepReportService(IUnitOfWork unitOfWork) : IEducationAccountSweepReportService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<EducationAccountSweepReportDTO> GetReportAsync(
            DateOnly? date,
            CancellationToken cancellationToken = default)
        {
            var reportDate = date ?? GetTodaySgtDate();
            var report = await _unitOfWork.Repository<EducationAccountSweepReport>()
                .Query()
                .Where(report => report.BatchDate == reportDate)
                .FirstOrDefaultAsync(cancellationToken);

            if (report == null)
            {
                return new EducationAccountSweepReportDTO
                {
                    BatchDate = reportDate.ToString("yyyy-MM-dd"),
                    TotalDuration = "00:00:00",
                    AccountsCreatedSuccessfully = 0,
                    AccountsFailedManualHandling = 0,
                    AccountsClosed = 0,
                    AccountsExtended = 0
                };
            }

            var failedCount = await _unitOfWork.Repository<EducationAccountSweepTarget>()
                .Query()
                .CountAsync(t => t.SweepReportId == report.Id && t.Status == SweepTargetStatus.Failed, cancellationToken);

            return new EducationAccountSweepReportDTO
            {
                BatchDate = report.BatchDate.ToString("yyyy-MM-dd"),
                TotalDuration = (report.CompletedAt - report.StartedAt).ToString(@"hh\:mm\:ss"),

                AccountsCreatedSuccessfully = report.AccountsCreatedCount,
                AccountsFailedManualHandling = failedCount,

                AccountsClosed = report.AccountsClosedCount,
                AccountsExtended = report.AccountsExtendedCount
            };
        }

        public async Task<PaginationResult<EducationAccountSweepTargetRecordDTO>> GetReportTargetsAsync(
            DateOnly batchDate,
            EducationAccountSweepTargetFilterDTO filter,
            CancellationToken cancellationToken = default)
        {
            var pageSize = Math.Clamp(filter.PageSize, 1, 100);
            var page = Math.Max(filter.Page, 1);
            var reportId = await _unitOfWork.Repository<EducationAccountSweepReport>()
                .Query()
                .Where(r => r.BatchDate == batchDate)
                .Select(r => r.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (reportId == 0)
            {
                return new PaginationResult<EducationAccountSweepTargetRecordDTO>(
                    0,
                    pageSize,
                    []);
            }

            var query = _unitOfWork.Repository<EducationAccountSweepTarget>()
                .Query()
                .Where(t => t.SweepReportId == reportId);

            if (filter.Statuses?.Count > 0)
            {
                query = query.Where(t => filter.Statuses.Contains(t.Status));
            }

            if (filter.Actions?.Count > 0)
            {
                query = query.Where(t => filter.Actions.Contains(t.Action));
            }

            if (!string.IsNullOrWhiteSpace(filter.Nric))
            {
                query = query.Where(t => t.Nric.Contains(filter.Nric));
            }

            var total = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(target => target.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new EducationAccountSweepTargetRecordDTO
                {
                    Nric = x.Nric,
                    Action = x.Action.ToString(),
                    Status = x.Status.ToString(),
                    Reason = x.Reason ?? string.Empty
                }).ToListAsync(cancellationToken);

            return new PaginationResult<EducationAccountSweepTargetRecordDTO>(total, pageSize, items);
        }

        public async Task<EducationAccountSweepManualHandlingDTO> GetFailedRecordForManualHandlingAsync(
            string nric,
            DateOnly batchRunDate,
            CancellationToken cancellationToken = default)
        {
            var failedRecord = await _unitOfWork.Repository<EducationAccountSweepTarget>()
                .Query()
                .FirstOrDefaultAsync(x => x.SweepReport.BatchDate == batchRunDate && x.Nric == nric && x.Status == SweepTargetStatus.Failed, cancellationToken);

            if (failedRecord == null)
            {
                return new EducationAccountSweepManualHandlingDTO();
            }

            return new EducationAccountSweepManualHandlingDTO
            {
                Nric = failedRecord.Nric,
                BatchRunDate = batchRunDate.ToString("yyyy-MM-dd"),
                Note = $"NRIC was pre-filled from a failed batch run dated {batchRunDate.ToString("yyyy-MM-dd")}."
            };
        }

        private static DateOnly GetTodaySgtDate()
        {
            var sgtNow = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(8));
            return DateOnly.FromDateTime(sgtNow.DateTime);
        }
    }
}
