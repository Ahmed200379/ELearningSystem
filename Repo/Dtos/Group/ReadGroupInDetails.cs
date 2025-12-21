using Shared.Dtos.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Group
{
    public class ReadGroupInDetails
    {
        public string Id { get; set; }=string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal SubscriptionFee { get; set; }
        public  List<ReadMaterialDto> Materials { get; set; } = new List<ReadMaterialDto>();
    }
}
