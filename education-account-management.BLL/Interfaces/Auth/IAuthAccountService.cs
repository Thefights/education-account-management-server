using DTOs.Auth;

namespace Interfaces.Auth
{
    public interface IAuthAccountService
    {
        Task<GetAuthAccountProfileDTO> GetCurrentAuthAccountAsync(CancellationToken cancellationToken = default);

        Task<GetAuthAccountProfileDTO> UpdateCurrentAuthAccountAsync(
            UpdateAuthAccountProfileDTO updateDTO,
            CancellationToken cancellationToken = default);
    }
}
