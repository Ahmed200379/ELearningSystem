
using Microsoft.AspNetCore.Http;
using Shared.Dtos.Question;

namespace Shared.Dtos.Quiz
{
    public class RequestUpdateQuizeDto
    {
        public int NumberOfQuestions { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public double Points { get; set; } = 0.0;
        public List<RequestUpdateQuestionWithoutQuizIdDto> Questions { get; set; } = new();
    }
}
