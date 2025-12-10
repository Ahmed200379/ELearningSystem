
using Shared.Dtos.Answer;
using Shared.Enums;

namespace Shared.Dtos.Question
{
    public class ResponseQuestionDto
    {
        public string Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
        public QuestionType Type { get; set; } = QuestionType.Choose;
        public int TimeLimit { get; set; } = 0;
        public string QuizId { get; set; }
        public List<ResponseAnswerWithoutQuestionIdDto> Answers { get; set; } = new List<ResponseAnswerWithoutQuestionIdDto>();
    }
}
