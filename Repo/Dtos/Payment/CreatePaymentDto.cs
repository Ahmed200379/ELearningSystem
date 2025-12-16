using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Payment
{
    public class CreatePaymentDto
    {
        [Required]
        public string StudentId { get; set; } = string.Empty;
        [Required]
        public string GroupId { get; set; } = string.Empty;
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Currency { get; set; } = string.Empty;
    }
}
