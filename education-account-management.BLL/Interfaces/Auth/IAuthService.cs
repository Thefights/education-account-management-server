using DTOs.Admin;
using DTOs.Auth;

namespace Interfaces.Auth
{
    public interface IAuthService
    {
        Task UpdateStatusAsync(UpdateAdminStatusDTO dto,
            CancellationToken cancellationToken = default);

        Task<AuthLoginResponseDTO> LoginWithMockSingpassAsync(
           CancellationToken cancellationToken = default);

        Task<AuthLoginResponseDTO> LoginWithAzureAdAsync(
            AzureAdLoginRequestDTO request,
            CancellationToken cancellationToken = default);

        Task LogoutAsync(
            string refreshToken,
            string accessToken,
            CancellationToken cancellationToken = default);

        Task<AuthLoginResponseDTO> RefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken = default);
    }
}
