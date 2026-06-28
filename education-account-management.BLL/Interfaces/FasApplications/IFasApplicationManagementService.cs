using DTOs.FasApplications;
using Filters.FasApplications;
using Results;

namespace Interfaces.FasApplications
{
    public interface IFasApplicationManagementService
    {
        Task<PaginationResult<GetFasApplicationSchoolAdminDTO>> GetApplicationPaginatedAsync(FasApplicationFilterDTO request, CancellationToken cancellationToken = default);
        Task<GetFasApplicationSchoolAdminDetailDTO> GetApplicationDetailsAsync(int applicationId, CancellationToken cancellationToken = default);
        Task ApproveAsync( int id, CancellationToken cancellationToken = default);
        Task RejectAsync(int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken = default);
    }
}
