namespace Infrastructure.Interface
{
    public interface IRefreshTokenCookieService
    {
        string? Get();

        void Set(string refreshToken, DateTimeOffset? expires = null);

        void Clear();
    }
}
