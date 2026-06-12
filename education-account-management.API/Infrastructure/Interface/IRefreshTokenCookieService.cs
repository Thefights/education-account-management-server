using DTOs.Auth;

namespace Infrastructure.Interface
{
    public interface IRefreshTokenCookieService
    {
        string? RefreshToken { get; }

        void Set(AuthTokenResponseDTO tokens);

        void Clear();
    }
}
