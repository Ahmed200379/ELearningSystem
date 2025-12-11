using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Material
{
    public class ReadMaterialDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;
        public DateTime AdditionDate { get; set; }
        public TypeOfMaterial Type { get; set; }
        public string GroupId { get; set; }
    }
}
