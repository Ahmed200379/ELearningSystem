
using Shared.Dtos.Question;

namespace Shared.Dtos.Quiz
{
    public class ResponseQuizDto
    {
        public string Id { get; set; }
        public int NumberOfQuestions { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public double Points { get; set; } = 0.0;
        public List<ResponseQuestionWithExamDto> Questions { get; set; } = new();
    }
}
