namespace DTOs.Schools
{
    public class CreateSchoolDTO
    {
        public string SchoolName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }

    public class UpdateSchoolDTO
    {
        public string SchoolName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }

    public class GetSchoolDTO
    {
        public int Id { get; set; }

        public string? Status { get; set; }

        public string SchoolName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }

    public class BatchUpdateSchoolStatusDTO
    {
        public List<int> Ids { get; set; } = [];
        public int Status { get; set; }
    }
}