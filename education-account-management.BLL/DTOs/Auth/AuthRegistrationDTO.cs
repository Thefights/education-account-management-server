using System.Text.Json.Serialization;

namespace DTOs.Auth
{
    public class RegisterRequestDTO
    {
        public string? UserId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

        public string? PhoneNumber { get; set; }

        public UserGender? Gender { get; set; }

        public string? EmailVerificationSessionId { get; set; }
    }

    public class SendRegisterEmailOtpRequestDTO
    {
        public string? Email { get; set; }
    }

    public class SendRegisterEmailOtpResponseDTO
    {
        public string SessionId { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }
    }

    public class VerifyRegisterEmailOtpRequestDTO
    {
        public string? SessionId { get; set; }

        public string? Email { get; set; }

        public string? OtpCode { get; set; }
    }
}
