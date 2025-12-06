using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   public class Answer :Base
    {
        public string Text { get; set; }=string.Empty;
        public string? Image {  get; set; }=string.Empty;
        public bool IsCorrect { get; set; } =false;
        public string QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
