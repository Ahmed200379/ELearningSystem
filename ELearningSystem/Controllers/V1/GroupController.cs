using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize( Roles = "Admin,Teacher,SuperAdmin,Student")]
        [HttpGet("GetAll")]
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
        [Authorize(Roles = "Admin,Teacher,SuperAdmin,Student")]
        [HttpGet("getByCourseName")]
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
        [Authorize(Roles = "Admin,Teacher,SuperAdmin,Student")]
        [HttpGet("getAllInPagination/{pageNumber}/{pageSize}")]
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
        [Authorize(Policy = "GroupAccessPolicy")]
        [HttpGet("GetById/{groupId}")]
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
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("Delete/{groupId}")]
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
        [Authorize(Roles ="Admin,Student")]
        [HttpPost("CreateGroup")]
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
        [Authorize(Roles = "Admin,Teacher,SuperAdmin")]
        [HttpPut("UpdateGroup")]
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
