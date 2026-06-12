namespace DTOs.Email
{
    public class SaveEmailWhitelistDTO
    {
        public string? Values { get; set; }
    }

    public class GetEmailWhitelistDTO
    {
        public int Id { get; set; }

        public string? Value { get; set; }
    }
}
