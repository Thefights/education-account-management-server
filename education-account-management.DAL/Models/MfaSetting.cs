namespace Models
{
    public class MfaSetting : AuditEntity
    {
        public bool IsEnabled { get; set; }

        public bool EmailEnabled { get; set; }

        public bool SmsEnabled { get; set; }
    }
}
