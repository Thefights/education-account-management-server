using DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Auth
{
    public class CreateAuthAccountDTO : IUploadImageDTO
    {
        public string? UserIdText { get; set; }

        public string? Email { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public UserGender Gender { get; set; }

        [AllowFileType(FileType.Image)]
        public IFormFile? ImageUrl { get; set; }

        [MinLength(1)]
        public List<int> RoleIds { get; set; } = [];

    }

    public class GetAuthAccountDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? UserIdText { get; set; }

        public string? Email { get; set; }

        public string? Status { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Gender { get; set; }

        public string? ImageUrl { get; set; }

        public List<int> RoleIds { get; set; } = [];

    }

    public class GetAuthAccountProfileDTO
    {
        public string? UserIdText { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Gender { get; set; }

        public string? Status { get; set; }

        public string? ImageUrl { get; set; }
    }

    public class UpdateAuthAccountDTO : IUploadImageDTO
    {
        public string? Email { get; set; }

        public AuthAccountStatus Status { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public UserGender Gender { get; set; }

        [AllowFileType(FileType.Image)]
        public IFormFile? ImageUrl { get; set; }

        [MinLength(1)]
        public List<int> RoleIds { get; set; } = [];

    }

    public class UpdateAuthAccountProfileDTO : IUploadImageDTO
    {
        public string? UserIdText { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public UserGender Gender { get; set; }

        [AllowFileType(FileType.Image)]
        public IFormFile? ImageUrl { get; set; }
    }

    public class UpdateAuthAccountsStatusDTO
    {
        [MinLength(1)]
        public List<int> AuthAccountIds { get; set; } = [];

        [EnumDefined]
        public AuthAccountStatus Status { get; set; }
    }

    public class UnlockAuthAccountsDTO
    {
        [MinLength(1)]
        public List<int> AuthAccountIds { get; set; } = [];
    }
}
