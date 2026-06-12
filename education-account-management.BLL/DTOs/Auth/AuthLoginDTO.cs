using System.Text.Json.Serialization;

namespace DTOs.Auth
{
    public class LoginRequestDTO
    {
        public string? UserId { get; set; }

        public string? Password { get; set; }

        public bool StaySignedIn { get; set; }
    }

    public class AuthTokenResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;

        public DateTime AccessTokenExpiresAt { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpiresAt { get; set; }
    }

    public class LoginResponseDTO
    {
        public bool MfaRequired { get; set; }

        public string? SessionId { get; set; }

        public AuthTokenResponseDTO? Tokens { get; set; }
    }
}
