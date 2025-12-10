
using Microsoft.AspNetCore.Http;

namespace Shared.Dtos.Answer
{
    public class RequestAnswerDto
    {
        public string Text { get; set; } 
        public IFormFile? Image { get; set; }
        public bool IsCorrect { get; set; }
        public string QuestionId { get; set; }
    }
}
