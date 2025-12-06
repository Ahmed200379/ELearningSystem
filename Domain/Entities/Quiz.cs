using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   public class Quiz:Base
    {
        public int NumberOfQuestions { get; set; } = 0;
        public string Description { get; set; }=string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public double Points { get; set; } = 0.0;
        public string Image { get; set; } = string.Empty;
        public virtual ICollection<UserQuiz> UserQuizzes { get; set; } = new List<UserQuiz>();
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
