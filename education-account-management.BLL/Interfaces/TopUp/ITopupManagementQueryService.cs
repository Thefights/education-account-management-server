using DTOs.TopUp;
using Filters.TopUp;
using Results;

namespace Interfaces.TopUp
{
    public interface ITopupManagementQueryService
    {
        Task<PaginationResult<TopupEligibleAccountDTO>> GetEligibleAccountsAsync(TopupAccountLookupFilterDTO filter, CancellationToken cancellationToken = default);
        Task<PaginationResult<TopupExecutionDTO>> GetHistoryAsync(TopupExecutionFilterDTO filter, CancellationToken cancellationToken = default);
        Task<TopupExecutionDTO> GetHistoryByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PaginationResult<TopupExecutionTargetDTO>> GetTargetsAsync(int executionId, TopupExecutionTargetFilterDTO filter, CancellationToken cancellationToken = default);
    }
}
