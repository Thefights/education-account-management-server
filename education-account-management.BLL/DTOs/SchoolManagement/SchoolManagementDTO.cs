namespace DTOs.SchoolManagement
{
    public class CreateSchoolManagementDTO
    {
        public string SchoolName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }

    public class UpdateSchoolManagementDTO
    {
        public string SchoolName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }

    public class GetSchoolManagementDTO
    {
        public int Id { get; set; }

        public string? Status { get; set; }

        public string SchoolName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}