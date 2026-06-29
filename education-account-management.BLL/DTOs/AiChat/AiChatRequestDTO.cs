using System.Text.Json.Serialization;

namespace DTOs.AiChat
{
    public class AiChatRequestDTO
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = null!;

        [JsonPropertyName("history")]
        public List<AiChatMessageDTO> History { get; set; } = new();
    }

    public class AiChatMessageDTO
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = null!;

        [JsonPropertyName("content")]
        public string Content { get; set; } = null!;
    }
}
