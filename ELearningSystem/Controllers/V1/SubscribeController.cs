using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Chat;
using Shared.Dtos.Subscribe;
using System.Threading.Tasks;

namespace ELearningSystem.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class SubscribeController : ControllerBase
    {
        private readonly ISubscribtionServices _subscribtionServices;
        public SubscribeController(ISubscribtionServices subscribtionServices)
        {
            _subscribtionServices = subscribtionServices;
        }
        [HttpPost("Subscribe/AddStudentToGroup")]
        public async Task<IActionResult> AddStudentToGroup(AddStudentDto addStudentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value.Errors).Select(er => er.ErrorMessage));
            }
            try
            {
                var result = await _subscribtionServices.AddStudentToGroup(addStudentDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("Subscribe/UpdateSubscribe")]
        public async Task<IActionResult> UpdateSubscribeManually(UpdateSubscribeDto updateSubscribeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value.Errors).Select(er => er.ErrorMessage));
            }
            try
            {
                var result = await _subscribtionServices.UpdateSubscribeManually(updateSubscribeDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("Subscribe/CheckSubscriptionStatus")]
        public async Task<IActionResult> CheckSubscriptionStatus(string userId, string groupId)
        {
            try
            {
                var result = await _subscribtionServices.CheckUserSubscriptionStatus(userId, groupId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("Subscribe/GetSubscribedGroups")]
        public async Task<IActionResult> GetSubscribedGroups(string userId)
        {
            try
            {
                var result = await _subscribtionServices.GetAllSubscribtionGroupsForStudent(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("Subscribe/GetSubscribedStudents")]
        public async Task<IActionResult> GetSubscribedStudents(string groupId)
        {
            try
            {
                var result = await _subscribtionServices.GetAllStudentsForSpecificGroup(groupId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
