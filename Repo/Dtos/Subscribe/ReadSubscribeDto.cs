using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Subscribe
{
    public class ReadSubscribeDto
    {
        public string GroupId { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}
