namespace DTOs.EducationAccounts
{
    public class GetEducationAccountDTO
    {
        public int Id { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public string Nric { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateOnly ExpectedClosingDate => DateOfBirth.AddYears(31);

        public int Age => DateTime.Today.Year - DateOfBirth.Year - (DateTime.Today.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);

        public bool IsSingaporean { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ResidentialAddress { get; set; }

        public string? MailingAddress { get; set; }
    }

    public class CreateEducationAccountDTO
    {
        [MessageRequired]
        [SingaporeNric]
        public string Nric { get; set; } = string.Empty;

        [MessageRequired]
        [MessageMinLength(20)]
        [MessageMaxLength(500)]
        public string Reason { get; set; } = string.Empty;

        // Internal fields used during CSV import to hold resolved data
        internal int? ResolvedCitizenId { get; set; }
        internal string? GeneratedAccountNumber { get; set; }
    }

    public class UpdateEducationAccountDTO
    {
        [EnumDefined]
        public EducationAccountStatus Status { get; set; }
    }

    public class EducationAccountSweepResultDTO
    {
        public DateOnly BatchDate { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime CompletedAt { get; set; }

        public int AccountsCreatedCount { get; set; }

        public int AccountsClosedCount { get; set; }

        public int AccountsExtendedCount { get; set; }

        public List<EducationAccountSweepTargetDTO> Targets { get; set; } = [];
    }

    public class EducationAccountSweepTargetDTO
    {
        public string Nric { get; set; } = string.Empty;

        public SweepAction Action { get; set; }

        public SweepTargetStatus Status { get; set; }

        public string? Reason { get; set; }
    }

    public class BatchUpdateEducationAccountStatusDTO
    {
        public List<int> Ids { get; set; } = [];
        public EducationAccountStatus Status { get; set; }

        [MessageRequired]
        [MessageMinLength(10)]
        [MessageMaxLength(500)]
        public string Reason { get; set; } = string.Empty;
    }
}
