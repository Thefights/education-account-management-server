using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DTOs.AiChat
{
    public class DynamicFasChatRequestDTO
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(200)]
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = null!;

        [Range(1, int.MaxValue)]
        [JsonPropertyName("fas_scheme_id")]
        public int FasSchemeId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(4000)]
        [JsonPropertyName("message")]
        public string Message { get; set; } = null!;

        [MaxLength(100)]
        [JsonPropertyName("questions")]
        public List<DynamicFasQuestionDTO> Questions { get; set; } = new();

        [JsonPropertyName("current_answers")]
        public Dictionary<string, string> CurrentAnswers { get; set; } = new();
    }

    public class DynamicFasQuestionDTO
    {
        [Range(1, int.MaxValue)]
        [JsonPropertyName("question_id")]
        public int QuestionId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(500)]
        [JsonPropertyName("question_text")]
        public string QuestionText { get; set; } = null!;

        [JsonPropertyName("is_required")]
        public bool IsRequired { get; set; }

        [MaxLength(1000)]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression("^(text|textarea|select)$")]
        [JsonPropertyName("type")]
        public string Type { get; set; } = "textarea";

        [MaxLength(100)]
        [JsonPropertyName("options")]
        public List<string> Options { get; set; } = new();
    }

    public class DynamicFasResetSessionRequestDTO
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(200)]
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = null!;
    }
}
