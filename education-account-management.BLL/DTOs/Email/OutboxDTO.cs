namespace DTOs.Email
{
    public class EmailOutboxPayloadDTO
    {
        public string ToEmail { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string HtmlBody { get; set; } = string.Empty;

        public string TextBody { get; set; } = string.Empty;
    }
}
