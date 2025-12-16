using Shared.Helpers.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Subscribe
{
    [NoSpaces]
    public class UpdateSubscribeDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }=string.Empty;
        [Required]
        public string GroupId { get; set; } = string.Empty;
        [Required]
        public int DurationInMonths { get; set; }
    }
}
