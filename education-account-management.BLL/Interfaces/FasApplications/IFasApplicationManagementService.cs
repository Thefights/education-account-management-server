using DTOs.FasApplications;
using Interfaces.Base;

namespace Interfaces.FasApplications
{
    public interface IFasApplicationManagementService : IBaseGetService<GetFasApplicationSchoolAdminDTO>
    {
        Task<GetFasApplicationSchoolAdminDetailDTO> GetApplicationDetailsAsync(int applicationId, CancellationToken cancellationToken = default);
        Task ApproveAsync(int id, ApproveFasApplicationDTO dto, CancellationToken cancellationToken = default);
        Task RejectAsync(int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken = default);
    }
}
