using DTOs.Admin;
using Interfaces.Base;

namespace Interfaces
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