using DTOs.FasApplications;

using Filters.FasApplications;
using Results;

namespace Interfaces.FasApplications
{
    public interface IAccountHolderFasApplicationService
    {
        Task<string> SubmitApplicationAsync(SubmitFasApplicationDTO dto, CancellationToken cancellationToken = default);
        Task<PaginationResult<FasApplicationSummaryDTO>> GetMyApplicationsAsync(FasApplicationFilterDTO filter, CancellationToken cancellationToken = default);

        Task<FasApplicationDetailDTO> GetApplicationDetailAsync(int id, CancellationToken cancellationToken = default);

        Task WithdrawApplicationAsync(int id, CancellationToken cancellationToken = default);
    }
}
