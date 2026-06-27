using DTOs.FasApplications;
using Interfaces.Base;
using Results;

namespace Interfaces.FasApplications
{
    public interface IManagementFasApplicationService : IBaseGetService<FasApplicationDetailDTO>
    {
        Task ApproveAsync(int schoolId, int id, CancellationToken cancellationToken = default);
        Task RejectAsync(int schoolId, int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken = default);
    }
}
