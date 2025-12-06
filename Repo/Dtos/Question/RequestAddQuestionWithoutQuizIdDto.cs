using Shared.Dtos.Answer;
using Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace Shared.Dtos.Question
{
    public class RequestAddQuestionWithoutQuizIdDto
    {
        public string Title { get; set; } = string.Empty;
        public IFormFile? Image { get; set; } 
        public QuestionType Type { get; set; } = QuestionType.Choose;
        public int TimeLimit { get; set; } = 0;
        public  List<RequestAddAnswerWithoutQuestionIdDto> Answers { get; set; }
    }
}
