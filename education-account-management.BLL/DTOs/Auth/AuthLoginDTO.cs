using System.Text.Json.Serialization;

namespace DTOs.Auth
{
    public class AzureAdLoginRequestDTO
    {
        [MessageRequired, MessageMaxLength(5000)]
        public string IdToken { get; set; } = string.Empty;
    }

    public class AuthLoginResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;

        public DateTime AccessTokenExpiresAt { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpiresAt { get; set; }

        public int AuthAccountId { get; set; }

        public int UserId { get; set; }

        public string? Role { get; set; }
    }
}
