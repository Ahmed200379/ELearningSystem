using Domain.Entities;
using Domain.Interfaces;
using ELearningSystem;
using Microsoft.AspNetCore.SignalR;
using Services.Abstractions;
using Shared.Dtos;
using Shared.Dtos.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ChatServices :IChatServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub> _hubContext;
        public ChatServices(IUnitOfWork unitOfWork,IHubContext<ChatHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }
        public async Task<GeneralResponseDto> DeleteMessage(DeleteMessageDto deleteMessageDto)
        {
            var message = await _unitOfWork.GetRepository<Chat>().GetByIdAsync(deleteMessageDto.MessageId);
            if (message==null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Message not found",
                };
            }
            if (deleteMessageDto.UserId != message.UserId)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "You are not authorized to edit this message",
                };
            }
            _unitOfWork.GetRepository<Chat>().Delete(message);
            var result = await _unitOfWork.SaveChanges();
            if (result==0)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Failed to delete message",
                };
            }
            await _hubContext.Clients.Group(message.GroupId).SendAsync("DeleteMessage", deleteMessageDto.MessageId);
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Message deleted successfully",
            };
        }

        public async Task<GeneralResponseDto> EditMessage(EditMessageDto editMessageDto)
        {
            var message= await _unitOfWork.GetRepository<Chat>().GetByIdAsync(editMessageDto.MessageId);
            if (message==null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Message not found",
                };
            }
            if (editMessageDto.UserId!=message.UserId)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "You are not authorized to edit this message",
                };
            }
            message.Message = editMessageDto.NewContent;
            _unitOfWork.GetRepository<Chat>().Update(message);
            var result = await _unitOfWork.SaveChanges();
            if (result==0)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Failed to edit message",
                };
            }
            await _hubContext.Clients.Group(message.GroupId).SendAsync("UpdateMessage", message);
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Message edited successfully",
            };
        }

        public async Task<GeneralResponseDto> GetMessages(string groupId)
        {
            var messages = await _unitOfWork.GetRepository<Chat>().GetAllAsyncs(c => c.GroupId == groupId);
            messages = messages.OrderBy(c => c.SendAt).ToList();
            if (messages==null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "No messages found",
                };
            }
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Messages retrieved successfully",
                data = messages
            };
        }

        public async Task<GeneralResponseDto> SendMessage(SendMessageDto sendMessageDto)
        {
            var chatMessage = new Chat
            {
                GroupId = sendMessageDto.GroupId,
                UserId = sendMessageDto.UserId,
                Message = sendMessageDto.Message,
                SendAt = DateTime.UtcNow

            };
           await _unitOfWork.GetRepository<Chat>().AddAsync(chatMessage);
            var result = await _unitOfWork.SaveChanges();
            if (result==0)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Failed to send message",
                };
            }
            await _hubContext.Clients.Group(sendMessageDto.GroupId).SendAsync("ReceiveMessage", chatMessage);
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Message sent successfully",
            };
        }
    }
}
