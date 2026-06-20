using Exceptions;
using DTOs.EducationAccounts;
using Enums;
using Filters.EducationAccounts;
using Interfaces.Base;
using Interfaces.EducationAccounts;
using Microsoft.EntityFrameworkCore;
using Models;
using Results;
using Utils;

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
            var query = _unitOfWork.Repository<EducationAccountSweepReport>().Query();

            if (date.HasValue)
            {
                query = query.Where(x => x.BatchDate == date.Value);
            }

            var report = await query
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (report == null)
            {
                throw new DataNotFoundException("No report data available for this date. The batch job runs at 00:00 SGT — please check back later.");
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
            var reportId = await _unitOfWork.Repository<EducationAccountSweepReport>()
                .Query()
                .Where(r => r.BatchDate == batchDate)
                .Select(r => r.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (reportId == 0)
            {
                throw new DataNotFoundException("Report not found for the given date.");
            }

            var query = _unitOfWork.Repository<EducationAccountSweepTarget>()
                .Query()
                .Where(t => t.SweepReportId == reportId);

            if (filter.Status.HasValue)
            {
                query = query.Where(t => t.Status == filter.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Nric))
            {
                query = query.Where(t => t.Nric.Contains(filter.Nric));
            }

            var total = await query.CountAsync(cancellationToken);
            var pageSize = filter.PageSize > 0 ? filter.PageSize : 10;
            var page = filter.Page > 0 ? filter.Page : 1;

            var items = await query
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
            var report = await GetReportAsync(batchRunDate, cancellationToken);

            var failedRecord = await _unitOfWork.Repository<EducationAccountSweepTarget>()
                .Query()
                .FirstOrDefaultAsync(x => x.SweepReport.BatchDate == batchRunDate && x.Nric == nric && x.Status == SweepTargetStatus.Failed, cancellationToken);

            if (failedRecord == null)
            {
                throw new DataNotFoundException("Failed batch record not found.");
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
