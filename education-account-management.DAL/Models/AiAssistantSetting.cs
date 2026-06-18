namespace Models
{
    public class AiAssistantSetting : BaseEntity
    {
        public bool IsEnabled { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [NotDefaultValue]
        public int UpdatedByUserId { get; set; }
        public User UpdatedByUser { get; set; } = null!;
    }
}
