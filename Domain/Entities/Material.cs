using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   public class Material:Base
    {
        public string Title { get; set; }=string.Empty;
        public string Description { get; set; } = string.Empty;
        public string File {  get; set; } = string.Empty;
        public TypeOfMaterial Type { get; set; } = TypeOfMaterial.Book;
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
