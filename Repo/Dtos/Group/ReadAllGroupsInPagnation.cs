using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Group
{
    public class ReadAllGroupsInPagnation
    {
        public List<ReadAllGroupsDto> Groups { get; set; } = new List<ReadAllGroupsDto>();
        public int TotalCount { get; set; }
    }
}
