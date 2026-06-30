

namespace DTOs.FasSchemes
{
    public class GetFasSchemeAdditionalQuestionDTO
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
    }

    public class FasSchemeAdditionalQuestionRequestDTO
    {
        [MessageRequired, MessageMaxLength(500)]
        public string QuestionText { get; set; } = string.Empty;

        public bool IsRequired { get; set; }
    }
}
