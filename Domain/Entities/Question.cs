using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   public class Question:Base
    {
        public string Title { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
        public QuestionType Type { get; set; }=QuestionType.Choose;
        public int TimeLimit { get; set; } = 0;
        public string QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
