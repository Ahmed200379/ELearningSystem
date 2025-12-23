using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Chat;
using Shared.Dtos.Material;
using Shared.Dtos.Subscribe;

namespace ELearningSystem.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;
        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }
        [Authorize(Roles = "Admin,Teacher,SuperAdmin")]
        [HttpPost("Material/create")]
        public async Task<IActionResult> Create([FromForm] AddMaterialDto addMaterialDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage));
            }
            try
            {
                var result = await _materialService.AddMaterial(addMaterialDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles = "Admin,Teacher,SuperAdmin")]
        [HttpPost("Material/AddVideoFromYoutube")]
        public async Task<IActionResult> AddVideoFromYoutube([FromForm] AddMaterialFromYoutube addMaterialFromYoutube)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage));
            }
            try
            {
                var result = await _materialService.AddVideoFromYoutube(addMaterialFromYoutube);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Policy = "GroupAccessPolicy")]
        [HttpGet("Material/{groupId}")]
        public async Task<IActionResult> Get(string groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage));
            }
            try
            {
                var result = await _materialService.GetAllMaterial(groupId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [Authorize(Roles = "Admin,Teacher,SuperAdmin")]
        [HttpDelete("material/delete{materialId}")]
        public async Task<IActionResult> DeleteMessage(string materialId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage));
            }
            try
            {
                var result = await _materialService.DeleteMaterial(materialId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
