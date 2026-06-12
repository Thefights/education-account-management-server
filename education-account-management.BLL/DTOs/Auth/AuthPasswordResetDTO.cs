using System.Text.Json.Serialization;

namespace DTOs.Auth
{
    public class ForgotPasswordRequestDTO
    {
        public string? UserId { get; set; }
    }

    public class ResetPasswordRequestDTO
    {
        public string? Token { get; set; }

        public string? NewPassword { get; set; }

        public string? ConfirmPassword { get; set; }
    }
}
