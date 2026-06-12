using DTOs.Base;

namespace DTOs.User
{
    public class CreateUserDTO : IUploadImageDTO
    {
        public int AuthAccountId { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public UserGender Gender { get; set; }

        [AllowFileType(FileType.Image)]
        public IFormFile? ImageUrl { get; set; }
    }

    public class GetUserDTO
    {
        public int Id { get; set; }

        public int AuthAccountId { get; set; }

        public string? UserIdText { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Gender { get; set; }

        public string? Status { get; set; }

        public int FailedLoginCount { get; set; }

        public DateTime? LockedUntil { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public string? ImageUrl { get; set; }
    }

    public class UpdateUserDTO : IUploadImageDTO
    {
        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public UserGender Gender { get; set; }

        [AllowFileType(FileType.Image)]
        public IFormFile? ImageUrl { get; set; }
    }
}
