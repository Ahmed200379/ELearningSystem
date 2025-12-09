using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Chat
{
    public class EditMessageDto
    {
        public string MessageId { get; set; }=string.Empty;
        public string UserId { get; set; }=string.Empty;
        public string NewContent { get; set; }=string.Empty;
    }
}
