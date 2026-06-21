using DTOs.Admin;
using Interfaces.Base;

namespace Interfaces.Admin
{
    public interface IAdminService : IBaseGetService<GetAdminDTO>
    {
        Task<GetAdminDTO> CreateAsync(
            CreateAdminDTO createDTO,
            CancellationToken cancellationToken = default);

        Task<GetAdminDTO> UpdateAsync(
            int userId,
            UpdateAdminDTO updateDTO,
            CancellationToken cancellationToken = default);

        Task UpdateAdminsStatusAsync(
            BatchUpdateAdminStatusDTO dto,
            CancellationToken cancellationToken = default);
    }
}