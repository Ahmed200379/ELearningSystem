using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.role
{
    public class AddRoleDto
    {
        [Required]
        public Role role {  get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
