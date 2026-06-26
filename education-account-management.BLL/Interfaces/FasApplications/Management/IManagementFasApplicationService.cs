using DTOs.FasApplications;

namespace Interfaces.FasApplications.Management
{
    public interface IManagementFasApplicationService
    {
        Task<FasApplicationQueueResponseDTO> GetApplicationQueueAsync(GetFasApplicationListRequestDTO request, int adminSchoolId, CancellationToken cancellationToken = default);
        Task<FasApplicationDetailsDTO> GetApplicationDetailsAsync(int applicationId, int adminSchoolId, CancellationToken cancellationToken = default);
    }
}
