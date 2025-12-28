using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Material
{
    public class ShowHomeworkDto
    {
        public DateTime UploadTimeFrom { get; set; }
        public DateTime UploadTimeTo{ get; set; }
        public string GroupId { get; set; } = string.Empty;
    }
}
