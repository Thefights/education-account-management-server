using DTOs.FasApplications;

namespace Interfaces.FasApplications
{
    public interface IManagementFasApplicationService
    {
        Task ApproveAsync(int id, CancellationToken cancellationToken = default);
        Task RejectAsync(int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken = default);
    }
}
