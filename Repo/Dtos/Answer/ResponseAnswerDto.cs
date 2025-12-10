
using Microsoft.AspNetCore.Http;

namespace Shared.Dtos.Answer
{
    public class ResponseAnswerDto
    {
        public string Text { get; set; } 
        public string? Image { get; set; } 
        public bool IsCorrect { get; set; } 
        public string QuestionId { get; set; }
    }
}
