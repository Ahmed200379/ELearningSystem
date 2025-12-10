
using Microsoft.AspNetCore.Http;
using Shared.Dtos.Answer;
using Shared.Enums;

namespace Shared.Dtos.Question
{
    public class RequestQuestionDto
    {
        public string Title { get; set; } = string.Empty;
        public IFormFile? Image { get; set; } = string.Empty;
        public QuestionType Type { get; set; } = QuestionType.Choose;
        public int TimeLimit { get; set; } = 0;
        public string QuizId { get; set; }
        public ICollection<RequestAddAnswerWithoutQuestionIdDto> Answers { get; set; } = new List<RequestAddAnswerWithoutQuestionIdDto>();
    }
}
