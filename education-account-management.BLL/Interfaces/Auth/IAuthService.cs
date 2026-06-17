using DTOs.Auth;

namespace Interfaces.Auth
{
    public interface IAuthService
    {
        Task<AuthLoginResponseDTO> LoginWithMockSingpassAsync(
            CancellationToken cancellationToken = default);

        Task<AuthLoginResponseDTO> LoginWithAzureAdAsync(
            AzureAdLoginRequestDTO request,
            CancellationToken cancellationToken = default);
    }
}
