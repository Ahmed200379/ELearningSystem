using Shared.Dtos;
using Shared.Dtos.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IChatServices
    {
         Task<GeneralResponseDto> SendMessage(SendMessageDto sendMessageDto);
         Task<GeneralResponseDto> GetMessages(string groupId);
         Task<GeneralResponseDto> DeleteMessage(DeleteMessageDto deleteMessageDto);
         Task<GeneralResponseDto> EditMessage(EditMessageDto editMessageDto);
    }
}
