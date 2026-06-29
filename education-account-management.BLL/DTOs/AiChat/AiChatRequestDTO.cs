using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DTOs.AiChat
{
    public class AiChatRequestDTO
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(4000)]
        [JsonPropertyName("message")]
        public string Message { get; set; } = null!;

        [MaxLength(50)]
        [JsonPropertyName("history")]
        public List<AiChatMessageDTO> History { get; set; } = new();
    }

    public class AiChatMessageDTO
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(20)]
        [JsonPropertyName("role")]
        public string Role { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        [MaxLength(4000)]
        [JsonPropertyName("content")]
        public string Content { get; set; } = null!;
    }
}
