using education_account_management.BLL;
using Enums;
using Microsoft.IdentityModel.Tokens;
using Models;
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

        public static string CreateAccessToken(AppConfiguration configuration, User user, DateTime expiresAt)
        {
            return CreateAccessToken(
                configuration,
                user.AuthAccountId,
                user.Id,
                user.Role,
                ResolveDisplayName(user),
                expiresAt);
        }

        public static string CreateAccessToken(
            AppConfiguration configuration,
            int authAccountId,
            int userId,
            UserRole role,
            string? name,
            DateTime expiresAt)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new("AuthId", authAccountId.ToString()),
                new("UserId", userId.ToString()),
                new(ClaimTypes.Role, role.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
            };

            if (!string.IsNullOrWhiteSpace(name))
            {
                claims.Add(new Claim(ClaimTypes.Name, name));
            }

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

        private static string? ResolveDisplayName(User user)
        {
            return user.AdminProfile?.FullName
                ?? user.Citizen?.FullName;
        }
    }
}
