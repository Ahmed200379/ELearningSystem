using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Chat;

namespace ELearningSystem.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ChatController : ControllerBase
    {
        private readonly IChatServices _chatServices;
        public ChatController(IChatServices chatServices)
        {
            _chatServices = chatServices;
        }
        [Authorize(Roles = "Admin,Teacher,SuperAdmin,Student")]
        [HttpPost("SendMessage")]
        public async Task<IActionResult> Create([FromBody] SendMessageDto sendMessageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage));
            }
            try
            {
                var result = await _chatServices.SendMessage(sendMessageDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Policy = "GroupAccessPolicy")]
        [HttpGet("GetMessages/{groupId}")]
        public async Task<IActionResult> GetMessages(string groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage));
            }
            try
            {
                var result = await _chatServices.GetMessages(groupId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [Authorize(Roles = "Admin,Teacher,SuperAdmin,Student")]
        [HttpGet("GetMessageById/{chatId}")]
        public async Task<IActionResult> GetMessageById(string chatId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage));
            }
            try
            {
                var result = await _chatServices.GetMessageById(chatId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles = "Admin,Teacher,SuperAdmin,Student")]
        [HttpPut("EditMessage")]
        public async Task<IActionResult> EditMessage([FromBody] EditMessageDto editMessageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage));
            }
            try
            {
                var result = await _chatServices.EditMessage(editMessageDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles = "Admin,Teacher,SuperAdmin")]
        [HttpDelete("DeleteMessage")]
        public async Task<IActionResult> DeleteMessage([FromBody] DeleteMessageDto deleteMessageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage));
            }
            try
            {
                var result = await _chatServices.DeleteMessage(deleteMessageDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
