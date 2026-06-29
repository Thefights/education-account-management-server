namespace Models
{
    public class FasApplicationAdditionalQuestionAnswer : AuditEntity
    {
        // Application sở hữu câu trả lời bổ sung này.
        [NotDefaultValue]
        public int FasApplicationId { get; set; }
        public FasApplication FasApplication { get; set; } = null!;

        // Câu hỏi cấu hình từ scheme; nullable để snapshot vẫn còn nếu câu hỏi gốc không còn được tham chiếu.
        public int? FasSchemeAdditionalQuestionId { get; set; }
        public FasSchemeAdditionalQuestion? FasSchemeAdditionalQuestion { get; set; }

        // Nội dung câu hỏi snapshot để lịch sử không đổi khi scheme question đổi nội dung.
        [MessageRequired, MessageMaxLength(500)]
        public string QuestionTextSnapshot { get; set; } = string.Empty;

        // Required flag snapshot tại thời điểm student lưu/nộp application.
        public bool IsRequiredSnapshot { get; set; }

        // Câu trả lời text của student.
        [MessageMaxLength(2000)]
        public string? AnswerText { get; set; }
    }
}
