using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   public class Group:Base
    {
        public string Title { get; set; }=string.Empty;
        public string Description { get; set; }=string.Empty ;
        public string CourseName { get; set; } = string.Empty ;
        public DateTime CreatedAt { get; set; }=DateTime.Now;
        public decimal SubscriptionFee { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
        public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
        public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();
    }
}
