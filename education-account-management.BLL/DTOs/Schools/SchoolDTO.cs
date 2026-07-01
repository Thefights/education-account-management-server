using EntityAnnotations.RegExAttributes;

namespace DTOs.Schools
{
    public class CreateSchoolDTO
    {
        [MessageRequired, MessageMaxLength(150)]
        public string SchoolName { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(300)]
        public string Address { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(20), PhoneNumberValidator]
        public string PhoneNumber { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(320), EmailValidator]
        public string Email { get; set; } = string.Empty;
    }

    public class UpdateSchoolDTO
    {
        [MessageRequired, MessageMaxLength(150)]
        public string SchoolName { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(300)]
        public string Address { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(20), PhoneNumberValidator]
        public string PhoneNumber { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(320), EmailValidator]
        public string Email { get; set; } = string.Empty;
    }

    public class GetSchoolDTO
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

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

        [MessageRequired, MessageMinLength(10), MessageMaxLength(500)]
        public string Reason { get; set; } = string.Empty;
    }

}
