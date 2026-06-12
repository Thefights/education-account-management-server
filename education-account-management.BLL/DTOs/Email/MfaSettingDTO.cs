namespace DTOs.Email
{
    public class GetMfaSettingDTO
    {
        public int Id { get; set; }

        public bool IsEnabled { get; set; }

        public bool EmailEnabled { get; set; }

        public bool SmsEnabled { get; set; }
    }

    public class UpdateMfaSettingDTO
    {
        public bool IsEnabled { get; set; }

        public bool EmailEnabled { get; set; }

        public bool SmsEnabled { get; set; }
    }
}
