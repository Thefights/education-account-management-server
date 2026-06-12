namespace Models
{
    public class EmailWhitelist : AuditEntity
    {
        [MessageRequired, MessageMaxLength(320), Unique]
        public string Value { get; set; } = string.Empty;
    }
}
