using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   public class UserQuiz
    {
        public double Grade { get; set; } = 0.0;
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }

    }
}
