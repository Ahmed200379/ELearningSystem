
using Microsoft.AspNetCore.Http;

namespace Shared.Dtos.Answer
{
    public class RequestUpdateAnswerWithoutQuestionIdDto
    {
        public string Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public bool IsCorrect { get; set; }
    }
}
