using Microsoft.AspNetCore.Http;
using Shared.Dtos.Answer;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Question
{
    public class RequestUpdateQuestionWithoutQuizIdDto
    {
        public string Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public QuestionType Type { get; set; } = QuestionType.Choose;
        public int TimeLimit { get; set; } = 0;
        public List<RequestUpdateAnswerWithoutQuestionIdDto> Answers { get; set; }
    }
}
