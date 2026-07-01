namespace DTOs.EducationAccounts
{
    public class EducationAccountSweepDailyReportDTO
    {
        public string BatchDate { get; set; } = string.Empty;
        public string TotalDuration { get; set; } = string.Empty;

        public int AccountsCreatedSuccessfully { get; set; }
        public int AccountsFailedManualHandling { get; set; }

        public int AccountsClosed { get; set; }
        public int AccountsExtended { get; set; }
    }

    public class EducationAccountSweepReportSummaryDTO
    {
        public string? BatchDate { get; set; }

        public string? DateFrom { get; set; }

        public string? DateTo { get; set; }

        public string TotalDuration { get; set; } = string.Empty;

        public int AccountsCreatedSuccessfully { get; set; }

        public int AccountsFailedManualHandling { get; set; }

        public int AccountsClosed { get; set; }

        public int AccountsExtended { get; set; }

        public List<EducationAccountSweepDailyReportDTO> Reports { get; set; } = [];
    }

    public class EducationAccountSweepTargetRecordDTO
    {
        public string BatchDate { get; set; } = string.Empty;

        public string Nric { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string Reason { get; set; } = string.Empty;
    }

    public class EducationAccountSweepManualHandlingDTO
    {
        public string Nric { get; set; } = string.Empty;

        public string BatchRunDate { get; set; } = string.Empty;

        public string Note { get; set; } = string.Empty;
    }
}
