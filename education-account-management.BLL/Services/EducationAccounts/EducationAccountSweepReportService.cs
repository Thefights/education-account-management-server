using DTOs.EducationAccounts;
using Filters.EducationAccounts;
using Interfaces.EducationAccounts;
using Mappers.EducationAccounts;
using Results;

namespace Services.EducationAccounts
{
    public class EducationAccountSweepReportService(IUnitOfWork unitOfWork) : IEducationAccountSweepReportService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<EducationAccountSweepReportSummaryDTO> GetReportAsync(
            EducationAccountSweepReportQueryDTO query,
            CancellationToken cancellationToken = default)
        {
            var (dateFrom, dateTo, mode) = ResolveReportDateRange(query);

            if (mode == EducationAccountSweepReportDateFilterMode.Single)
            {
                var batchDate = dateFrom!.Value;
                var report = await _unitOfWork.Repository<EducationAccountSweepReport>()
                    .Query()
                    .FirstOrDefaultAsync(report => report.BatchDate == batchDate, cancellationToken);

                if (report == null)
                {
                    return EducationAccountSweepReportMapper.MapToEmptySingleSummary(batchDate);
                }

                var failedCount = await _unitOfWork.Repository<EducationAccountSweepTarget>()
                    .Query()
                    .CountAsync(t => t.SweepReportId == report.Id && t.Status == SweepTargetStatus.Failed, cancellationToken);

                return EducationAccountSweepReportMapper.MapToSingleSummary(report, failedCount);
            }

            var reportQuery = _unitOfWork.Repository<EducationAccountSweepReport>().Query();
            if (mode == EducationAccountSweepReportDateFilterMode.Range)
            {
                reportQuery = reportQuery.Where(report => report.BatchDate >= dateFrom!.Value && report.BatchDate <= dateTo!.Value);
            }

            var reports = await reportQuery
                .OrderBy(report => report.BatchDate)
                .ToListAsync(cancellationToken);

            var failedCounts = await GetFailedCountsAsync(reports.Select(report => report.Id).ToList(), cancellationToken);
            return EducationAccountSweepReportMapper.MapToRangeSummary(
                reports,
                failedCounts,
                mode == EducationAccountSweepReportDateFilterMode.Range ? dateFrom : null,
                mode == EducationAccountSweepReportDateFilterMode.Range ? dateTo : null);
        }

        public async Task<PaginationResult<EducationAccountSweepTargetRecordDTO>> GetReportTargetsAsync(
            DateOnly batchDate,
            EducationAccountSweepTargetFilterDTO filter,
            CancellationToken cancellationToken = default)
        {
            var pageSize = Math.Clamp(filter.PageSize, 1, 100);
            var page = Math.Max(filter.Page, 1);
            var query = _unitOfWork.Repository<EducationAccountSweepTarget>()
                .Query()
                .Where(t => t.SweepReport.BatchDate == batchDate);

            query = ApplyTargetFilters(query, filter);

            var total = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(target => target.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new EducationAccountSweepTargetRecordDTO
                {
                    BatchDate = x.SweepReport.BatchDate.ToString("yyyy-MM-dd"),
                    Nric = x.Nric,
                    Action = x.Action.ToString(),
                    Status = x.Status.ToString(),
                    Reason = x.Reason ?? string.Empty
                }).ToListAsync(cancellationToken);

            return new PaginationResult<EducationAccountSweepTargetRecordDTO>(total, pageSize, items);
        }

        public async Task<PaginationResult<EducationAccountSweepTargetRecordDTO>> GetReportTargetsAsync(
            EducationAccountSweepTargetRangeFilterDTO filter,
            CancellationToken cancellationToken = default)
        {
            var (dateFrom, dateTo, isAll) = ResolveTargetDateRange(filter);
            var pageSize = Math.Clamp(filter.PageSize, 1, 100);
            var page = Math.Max(filter.Page, 1);

            var query = _unitOfWork.Repository<EducationAccountSweepTarget>()
                .Query();

            if (!isAll)
            {
                query = query.Where(t => t.SweepReport.BatchDate >= dateFrom!.Value && t.SweepReport.BatchDate <= dateTo!.Value);
            }

            query = ApplyTargetFilters(query, filter);

            var total = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(target => target.SweepReport.BatchDate)
                .ThenBy(target => target.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new EducationAccountSweepTargetRecordDTO
                {
                    BatchDate = x.SweepReport.BatchDate.ToString("yyyy-MM-dd"),
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

        private async Task<Dictionary<int, int>> GetFailedCountsAsync(
            List<int> reportIds,
            CancellationToken cancellationToken)
        {
            if (reportIds.Count == 0)
            {
                return [];
            }

            return await _unitOfWork.Repository<EducationAccountSweepTarget>()
                .Query()
                .Where(target => reportIds.Contains(target.SweepReportId) && target.Status == SweepTargetStatus.Failed)
                .GroupBy(target => target.SweepReportId)
                .Select(group => new
                {
                    ReportId = group.Key,
                    Count = group.Count()
                })
                .ToDictionaryAsync(item => item.ReportId, item => item.Count, cancellationToken);
        }

        private static IQueryable<EducationAccountSweepTarget> ApplyTargetFilters(
            IQueryable<EducationAccountSweepTarget> query,
            EducationAccountSweepTargetFilterDTO filter)
        {
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

            return query;
        }

        private static (DateOnly? DateFrom, DateOnly? DateTo, EducationAccountSweepReportDateFilterMode Mode) ResolveReportDateRange(
            EducationAccountSweepReportQueryDTO? query)
        {
            query ??= new EducationAccountSweepReportQueryDTO();
            var hasAll = query.All;
            var hasSingleDate = query.Date.HasValue;
            var hasDateFrom = query.DateFrom.HasValue;
            var hasDateTo = query.DateTo.HasValue;
            var hasRange = hasDateFrom || hasDateTo;

            if (hasAll && (hasSingleDate || hasRange))
            {
                throw new ValidationFailureException(new Dictionary<string, string>
                {
                    [nameof(query.All)] = "Use either all or date/dateFrom/dateTo, not both.",
                    [nameof(query.Date)] = "Use either all or date/dateFrom/dateTo, not both.",
                    [nameof(query.DateFrom)] = "Use either all or date/dateFrom/dateTo, not both.",
                    [nameof(query.DateTo)] = "Use either all or date/dateFrom/dateTo, not both."
                });
            }

            if (hasSingleDate && hasRange)
            {
                throw new ValidationFailureException(new Dictionary<string, string>
                {
                    [nameof(query.Date)] = "Use either date or dateFrom/dateTo, not both.",
                    [nameof(query.DateFrom)] = "Use either date or dateFrom/dateTo, not both.",
                    [nameof(query.DateTo)] = "Use either date or dateFrom/dateTo, not both."
                });
            }

            if (hasRange)
            {
                var (dateFrom, dateTo) = ResolveRequiredDateRange(query.DateFrom, query.DateTo);
                return (dateFrom, dateTo, EducationAccountSweepReportDateFilterMode.Range);
            }

            if (hasSingleDate)
            {
                var reportDate = query.Date.GetValueOrDefault();
                return (reportDate, reportDate, EducationAccountSweepReportDateFilterMode.Single);
            }

            return (null, null, EducationAccountSweepReportDateFilterMode.All);
        }

        private static (DateOnly? DateFrom, DateOnly? DateTo, bool IsAll) ResolveTargetDateRange(
            EducationAccountSweepTargetRangeFilterDTO filter)
        {
            var hasAll = filter.All;
            var hasDateFrom = filter.DateFrom.HasValue;
            var hasDateTo = filter.DateTo.HasValue;
            var hasRange = hasDateFrom || hasDateTo;

            if (hasAll && hasRange)
            {
                throw new ValidationFailureException(new Dictionary<string, string>
                {
                    [nameof(filter.All)] = "Use either all or dateFrom/dateTo, not both.",
                    [nameof(filter.DateFrom)] = "Use either all or dateFrom/dateTo, not both.",
                    [nameof(filter.DateTo)] = "Use either all or dateFrom/dateTo, not both."
                });
            }

            if (!hasRange)
            {
                return (null, null, true);
            }

            var (dateFrom, dateTo) = ResolveRequiredDateRange(filter.DateFrom, filter.DateTo);
            return (dateFrom, dateTo, false);
        }

        private static (DateOnly DateFrom, DateOnly DateTo) ResolveRequiredDateRange(
            DateOnly? dateFrom,
            DateOnly? dateTo)
        {
            var errors = new Dictionary<string, string>();
            if (!dateFrom.HasValue)
            {
                errors[nameof(EducationAccountSweepReportQueryDTO.DateFrom)] = "Date from is required.";
            }

            if (!dateTo.HasValue)
            {
                errors[nameof(EducationAccountSweepReportQueryDTO.DateTo)] = "Date to is required.";
            }

            if (errors.Count > 0)
            {
                throw new ValidationFailureException(errors);
            }

            var resolvedDateFrom = dateFrom.GetValueOrDefault();
            var resolvedDateTo = dateTo.GetValueOrDefault();

            if (resolvedDateFrom > resolvedDateTo)
            {
                throw new ValidationFailureException(nameof(EducationAccountSweepReportQueryDTO.DateFrom), "Date from must be earlier than or equal to date to.");
            }

            return (resolvedDateFrom, resolvedDateTo);
        }
    }
}
