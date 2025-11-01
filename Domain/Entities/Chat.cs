using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   public class Chat :Base
    {
        public String Message { get; set; } = String.Empty;
        public DateTime SendAt {  get; set; }=DateTime.Now;
        public virtual Group Group { get; set; }
        public string GroupId { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }

    }
}
