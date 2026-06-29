namespace Models
{
    public class FasSchemeAdditionalQuestion : AuditEntity
    {
        // Scheme sở hữu câu hỏi bổ sung này.
        [NotDefaultValue]
        public int FasSchemeId { get; set; }
        public FasScheme FasScheme { get; set; } = null!;

        // Nội dung câu hỏi hiển thị cho student khi apply.
        [MessageRequired, MessageMaxLength(500)]
        public string QuestionText { get; set; } = string.Empty;

        // Cho biết student có bắt buộc trả lời câu hỏi này khi submit application hay không.
        public bool IsRequired { get; set; }

        // Các câu trả lời application đã snapshot từ câu hỏi này.
        [OnDelete(OnDeleteBehavior.Restrict)]
        public ICollection<FasApplicationAdditionalQuestionAnswer> ApplicationAnswers { get; set; } = [];
    }
}
