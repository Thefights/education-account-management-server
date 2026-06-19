using DTOs.AdminManagement;
using Interfaces.Base;

namespace Interfaces.AdminManagement
{
    public interface IAdminManagementService : IBaseGetService<GetAdminManagementDTO>
    {
        Task<GetAdminManagementDTO> CreateAsync(
            CreateAdminManagementDTO createDTO,
            CancellationToken cancellationToken = default);

        Task<GetAdminManagementDTO> UpdateAsync(
            int userId,
            UpdateAdminManagementDTO updateDTO,
            CancellationToken cancellationToken = default);
    }
}
