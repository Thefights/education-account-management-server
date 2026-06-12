namespace DTOs.Auth
{
    public class LoginMfaOtpDTO
    {
        public string Code { get; set; } = string.Empty;

        public string SessionId { get; set; } = string.Empty;

        public string Target { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }
    }

    public class VerifyMfaOtpRequestDTO
    {
        public string? SessionId { get; set; }

        public string? OtpCode { get; set; }

        public bool StaySignedIn { get; set; }
    }

    public class ResendMfaOtpRequestDTO
    {
        public string? SessionId { get; set; }
    }

    public class ResendMfaOtpResponseDTO
    {
        public string SessionId { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }
    }
}
