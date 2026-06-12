using AvepointMosPlatform.BLL;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Security
{
    public static class TokenUtil
    {
        public static string GenerateRefreshToken(int byteLength = 32)
        {
            var bytes = new byte[byteLength];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        public static string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        public static string CreateAccessToken(AppConfiguration configuration, AuthAccount authAccount, DateTime expiresAt)
        {
            var user = authAccount.User;
            var role = user.UserRoles.FirstOrDefault()?.Role?.Name
                ?? throw new InvalidOperationException($"User {user.Id} has no role.");

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new("AuthId", authAccount.Id.ToString()),
                new("UserId", user.Id.ToString()),
                new(ClaimTypes.Name, authAccount.UserIdText),
                new(ClaimTypes.Role, role),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.JwtConfig.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration.JwtConfig.Issuer,
                audience: configuration.JwtConfig.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
