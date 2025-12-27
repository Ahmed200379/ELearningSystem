using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.role;
using Shared.Enums;

namespace ELearningSystem.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "SuperAdmin")]
    public class RoleManagement : ControllerBase
    {
        private readonly IRoleManagementService _roleManagementService;
        public RoleManagement(IRoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }
        [Authorize]
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] AddRoleDto addRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(m => m.Value!.Errors).Select(e => e.ErrorMessage));
            }
            try
            {
                var result = await _roleManagementService.AddRole(addRoleDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [HttpGet("getall/{role}")]
        public async Task<IActionResult> GetAll(Role role)
        {
            try
            {
                var result = await _roleManagementService.GetAllUsersWithSpecificRole(role);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
