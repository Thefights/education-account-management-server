using DTOs.EducationAccounts;
using Filters.EducationAccounts;
using Results;
using Utils;

namespace Interfaces.EducationAccounts
{
    public interface IEducationAccountSweepReportService
    {
        Task<EducationAccountSweepReportDTO> GetReportAsync(
            DateOnly? date,
            CancellationToken cancellationToken = default);

        Task<PaginationResult<EducationAccountSweepTargetRecordDTO>> GetReportTargetsAsync(
            DateOnly batchDate,
            EducationAccountSweepTargetFilterDTO filter,
            CancellationToken cancellationToken = default);

        Task<EducationAccountSweepManualHandlingDTO> GetFailedRecordForManualHandlingAsync(
            string nric,
            DateOnly batchRunDate,
            CancellationToken cancellationToken = default);
    }
}
