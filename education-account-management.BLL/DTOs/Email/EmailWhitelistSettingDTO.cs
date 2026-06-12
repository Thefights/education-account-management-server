namespace DTOs.Email
{
    public class GetEmailWhitelistSettingDTO
    {
        public int Id { get; set; }

        public bool IsEnabled { get; set; }
    }

    public class UpdateEmailWhitelistSettingDTO
    {
        public bool IsEnabled { get; set; }
    }
}
