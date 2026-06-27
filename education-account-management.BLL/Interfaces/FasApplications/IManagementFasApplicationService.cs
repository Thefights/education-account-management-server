using DTOs.FasApplications;
using Filters.FasApplications;
using Results;

namespace Interfaces.FasApplications
{
    public interface IManagementFasApplicationService
    {
        Task<PaginationResult<FasApplicationSchoolAdminDTO>> GetApplicationPaginatedAsync(FasApplicationFilterDTO request, CancellationToken cancellationToken = default);
        Task<FasApplicationSchoolAdminDetailDTO> GetApplicationDetailsAsync(int applicationId, CancellationToken cancellationToken = default);
        Task ApproveAsync( int id, CancellationToken cancellationToken = default);
        Task RejectAsync(int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken = default);
    }
}
