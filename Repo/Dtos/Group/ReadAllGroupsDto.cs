using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Group
{
    public class ReadAllGroupsDto
    {
        public string Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public decimal SubscriptionFee { get; set; }
    }
}
