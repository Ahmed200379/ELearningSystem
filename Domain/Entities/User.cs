using Microsoft.AspNetCore.Identity;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   public class User :IdentityUser
    {
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; }= string.Empty;
        public string? photoUrl { get; set; }
        public string ParentNumber { get; set; } = string.Empty;
        public Role Role { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
        public virtual ICollection<UserQuiz> UserQuizzes { get; set; } = new List<UserQuiz>();
        public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
        public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
