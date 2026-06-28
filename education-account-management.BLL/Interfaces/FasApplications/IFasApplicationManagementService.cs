using DTOs.FasApplications;
using Filters.FasApplications;
using Interfaces.Base;
using Results;

namespace Interfaces.FasApplications
{
    public interface IFasApplicationManagementService : IBaseGetService<GetFasApplicationSchoolAdminDTO>
    {
        Task<GetFasApplicationSchoolAdminDetailDTO> GetApplicationDetailsAsync(int applicationId, CancellationToken cancellationToken = default);
        Task ApproveAsync( int id, CancellationToken cancellationToken = default);
        Task RejectAsync(int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken = default);
    }
}
