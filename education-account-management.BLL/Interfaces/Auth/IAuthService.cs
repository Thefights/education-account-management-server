using DTOs.Auth;

namespace Interfaces.Auth
{
    public interface IAuthService
    {
        Task<AuthLoginResponseDTO> LoginWithMockSingpassAsync(
            MockSingpassLoginRequestDTO request,
            CancellationToken cancellationToken = default);

        Task<AuthLoginResponseDTO> LoginWithAzureAdAsync(
            AzureAdLoginRequestDTO request,
            CancellationToken cancellationToken = default);
    }
}
