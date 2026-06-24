namespace DTOs.Admin
{
    public class CreateAdminDTO
    {
        public UserRole Role { get; set; }

        public string AzureObjectId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Nric { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public int? SchoolId { get; set; }
    }

    public class UpdateAdminDTO
    {
        public UserRole Role { get; set; }

        public string AzureObjectId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Nric { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public int? SchoolId { get; set; }
    }

    public class GetAdminDTO
    {
        public int UserId { get; set; }

        public string? Role { get; set; }

        public string? Status { get; set; }

        public string AzureObjectId { get; set; } = string.Empty;

        public string StaffCode { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Nric { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public int? SchoolId { get; set; }

        public string? SchoolName { get; set; }
    }

    public class BatchUpdateAdminStatusDTO
    {
        public List<int> Ids { get; set; } = [];
        public int Status { get; set; }
    }
}
