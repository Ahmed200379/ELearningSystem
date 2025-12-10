using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Chat
{
    public class ReadMessageDto
    {
        public string MessageId { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
