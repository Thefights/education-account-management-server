using Infrastructure.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure
{
    public class TokenBlacklistService(ICacheService redis) : ITokenBlacklistService
    {
        private static readonly TimeSpan UserBlacklistTtl = TimeSpan.FromDays(1);
        private const string BlacklistKeyPrefix = "blacklist:";
        private const string UserBlacklistKeyPrefix = "blacklist:user:";

        public async Task BlacklistAsync(string accessToken, DateTime? expiresAt = null)
        {
            var tokenData = this.ReadTokenData(accessToken);
            if (tokenData == null)
            {
                return;
            }

            var finalExpireTime = expiresAt ?? tokenData.ExpireAtUtc ?? DateTime.UtcNow.AddHours(1);

            var ttl = finalExpireTime - DateTime.UtcNow;
            if (ttl <= TimeSpan.Zero)
            {
                ttl = TimeSpan.FromMinutes(1);
            }

            await this.SetBlacklistEntryAsync(tokenData.Jti, ttl);
        }

        public async Task<bool> IsBlacklistedAsync(string accessToken)
        {
            var tokenData = this.ReadTokenData(accessToken);
            return tokenData != null && await this.ExistsBlacklistEntryAsync(tokenData.Jti);
        }

        public async Task BlacklistUserAsync(int userId)
        {
            await this.SetEntryAsync(this.BuildUserBlacklistKey(userId), "1", UserBlacklistTtl);
        }

        public async Task<bool> IsUserBlacklistedAsync(int userId)
        {
            return await this.ExistsEntryAsync(this.BuildUserBlacklistKey(userId));
        }

        public async Task UnblacklistUserAsync(int userId)
        {
            await redis.DeleteAsync(this.BuildUserBlacklistKey(userId));
        }

        private async Task SetBlacklistEntryAsync(string jti, TimeSpan ttl)
        {
            await this.SetEntryAsync(this.BuildBlacklistKey(jti), "1", ttl);
        }

        private async Task<bool> ExistsBlacklistEntryAsync(string jti)
        {
            return await this.ExistsEntryAsync(this.BuildBlacklistKey(jti));
        }

        private async Task SetEntryAsync(string key, string value, TimeSpan? ttl = null)
        {
            await redis.SetAsync(key, value, ttl);
        }

        private async Task<bool> ExistsEntryAsync(string key)
        {
            return await redis.ExistsAsync(key);
        }

        private TokenData? ReadTokenData(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();

            JwtSecurityToken? jwt;
            try
            {
                jwt = handler.ReadJwtToken(accessToken);
            }
            catch
            {
                return null;
            }

            var jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            return string.IsNullOrWhiteSpace(jti) ? null : new TokenData(jti, this.TryGetTokenExpireTime(jwt));
        }

        private DateTime? TryGetTokenExpireTime(JwtSecurityToken jwt)
        {
            var expClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
            return long.TryParse(expClaim, out var expUnix) ? DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime : null;
        }

        private string BuildBlacklistKey(string jti)
        {
            return $"{BlacklistKeyPrefix}{jti}";
        }

        private string BuildUserBlacklistKey(int userId)
        {
            return $"{UserBlacklistKeyPrefix}{userId}";
        }

        private sealed record TokenData(string Jti, DateTime? ExpireAtUtc);
    }
}
