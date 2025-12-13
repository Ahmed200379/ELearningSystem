using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Group;

namespace ELearningSystem.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupServices _groupServices;
        public GroupController(IGroupServices groupServices)
        {
            _groupServices = groupServices;
        }
        [HttpGet("group/getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _groupServices.GetAllGroups();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("group/getByCourseName")]
        public async Task<IActionResult> GetByName(string courseName)
        {
            try
            {
                var result = await _groupServices.GetGroupsByCourseName(courseName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("group/getAllInPagination{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetAllInPagination(int pageNumber,int pageSize)
        {
            try
            {
                var result = await _groupServices.GetAllInPagination(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("group/getById/{groupId}")]
        public async Task<IActionResult> GetById(string groupId)
        {
            try
            {
                var result = await _groupServices.GetGroupById(groupId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("group/delete/{groupId}")]
        public async Task<IActionResult> Delete(string groupId)
        {
            try
            {
                var result = await _groupServices.DeleteGroup(groupId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [HttpPost("group/creategroup")]
        public async Task<IActionResult> Create([FromBody]CreateGroupDto createGroupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(m => m.Value!.Errors).Select(e => e.ErrorMessage));
            }
            try
            {
                var result = await _groupServices.CreateGroup(createGroupDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("group/addstudent")]
        public async Task<IActionResult> Add([FromBody] AddStudentDto addStudentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(m => m.Value!.Errors).Select(e => e.ErrorMessage));
            }
            try
            {
                var result = await _groupServices.AddStudentToGroup(addStudentDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("group/updategroup")]
        public async Task<IActionResult> Update([FromBody] UpdateGroupDto updateGroupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(m => m.Value!.Errors).Select(e => e.ErrorMessage));
            }
            try
            {
                var result = await _groupServices.UpdateGroup(updateGroupDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
