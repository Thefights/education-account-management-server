namespace Models
{
    public class Citizen : AuditEntity
    {
        [MessageRequired, MessageMaxLength(9), Unique]
        public string Nric { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(256), Unique]
        public string SingpassSubjectId { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [MessageMaxLength(320), EmailValidator]
        public string? Email { get; set; }

        [MessageMaxLength(20), PhoneNumberValidator]
        public string? PhoneNumber { get; set; }

        [MessageMaxLength(300)]
        public string? ResidentialAddress { get; set; }

        [MessageMaxLength(300)]
        public string? MailingAddress { get; set; }

        public DateOnly DateOfBirth { get; set; }

        [EnumDefined]
        public CitizenshipStatus CitizenshipStatus { get; set; } = CitizenshipStatus.Active;

        [MessageMaxLength(50)]
        public string? SchoolingStatus { get; set; }

        [OnDelete(OnDeleteBehavior.SetNull)]
        public ICollection<User> Users { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Restrict)]
        public EducationAccount? EducationAccount { get; set; }
    }
}
