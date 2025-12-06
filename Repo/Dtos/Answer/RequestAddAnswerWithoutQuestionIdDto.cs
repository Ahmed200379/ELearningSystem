
using Microsoft.AspNetCore.Http;

namespace Shared.Dtos.Answer
{
    public class RequestAddAnswerWithoutQuestionIdDto
    {
        public string Text { get; set; } = string.Empty;
        public IFormFile? Image { get; set; } 
        public bool IsCorrect { get; set; } 
    }
}
