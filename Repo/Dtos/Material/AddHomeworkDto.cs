using Microsoft.AspNetCore.Http;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Material
{
    public class AddHomeworkDto
    {
        [Required]
        [MaxLength(50), MinLength(10)]
        public string NameOfStudent { get; set; } = string.Empty;
        [Required]
        [MaxLength(500), MinLength(20)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string GroupId { get; set; } = string.Empty;
    }
}
