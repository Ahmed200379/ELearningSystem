
namespace Shared.Dtos.Answer
{
    public class ResponseAnswerWithoutQuestionIdDto
    {
        public string Id { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;
    }
}
