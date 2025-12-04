using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Material
{
    public class AddMaterialDto
    {
        [Required]
        [MaxLength(50),MinLength(10)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(500), MinLength(20)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string File { get; set; } = string.Empty;
        public TypeOfMaterial Type { get; set; } = TypeOfMaterial.Book;
        [Required]
        public string GroupId { get; set; } = string.Empty;
    }
}
