using System.Text.Json.Serialization;

namespace DTOs.Auth
{
    public class SocialLoginRequestDTO
    {
        public SocialLoginProvider Provider { get; set; }

        public string? ProviderToken { get; set; }
    }

    public class SocialAuthProviderProfileDTO
    {
        public SocialLoginProvider Provider { get; set; }

        public string ProviderUserId { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool EmailVerified { get; set; }
    }
}
