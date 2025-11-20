using Shared.Helpers.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Auth
{
   public class LoginDto
    {
        [EmailAddress]
        [NoSpaces]
        public string Email { get; set; } = string.Empty;
        [NoSpaces]
        public string Password { get; set; } = string.Empty;
    }
}
