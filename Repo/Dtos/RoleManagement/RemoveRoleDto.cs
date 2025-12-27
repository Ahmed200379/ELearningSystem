using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.RoleManagement
{
    public class RemoveRoleDto
    {
        [Required]
        public Role role { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
