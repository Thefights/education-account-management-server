namespace Infrastructure.Interface;

public interface ITokenBlacklistService
{
    Task BlacklistAsync(string accessToken, DateTime? expiresAt = null);
    Task<bool> IsBlacklistedAsync(string accessToken);
    Task BlacklistUserAsync(int userId);
    Task<bool> IsUserBlacklistedAsync(int userId);
    Task UnblacklistUserAsync(int userId);
}
