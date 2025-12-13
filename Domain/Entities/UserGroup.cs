using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserGroup
    {
        public DateTime SupscriptionTime { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = false;
        public DateTime ExpirationDate { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
