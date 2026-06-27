using DTOs.FasApplications;

namespace Interfaces.FasApplications
{
    public interface IManagementFasApplicationService
    {

        Task<FasApplicationQueueResponseDTO> GetApplicationQueueAsync(GetFasApplicationListRequestDTO request, int adminSchoolId, CancellationToken cancellationToken = default);
        Task<FasApplicationDetailsDTO> GetApplicationDetailsAsync(int applicationId, int adminSchoolId, CancellationToken cancellationToken = default);
        Task ApproveAsync(int schoolId, int id, CancellationToken cancellationToken = default);
        Task RejectAsync(int schoolId, int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken = default);
    }
}
