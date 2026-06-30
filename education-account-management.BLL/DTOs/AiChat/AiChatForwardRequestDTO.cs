using System.Text.Json.Serialization;

namespace DTOs.AiChat
{
    public class AiChatForwardRequestDTO
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = null!;

        [JsonPropertyName("history")]
        public List<AiChatMessageDTO> History { get; set; } = new();

        [JsonPropertyName("user_id")]
        public string UserId { get; set; } = null!;

        [JsonPropertyName("role")]
        public string Role { get; set; } = null!;
    }
}
