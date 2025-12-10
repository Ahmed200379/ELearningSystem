using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Chat
{
    public class SendMessageDto
    {
        [Required]
        public string Message { get; set; } = String.Empty;
        [Required]
        public string UserId { get; set; }
        [Required]
        public string GroupId { get; set; }
    }
}
