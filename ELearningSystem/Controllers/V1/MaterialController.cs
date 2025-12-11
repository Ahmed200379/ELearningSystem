using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Chat;
using Shared.Dtos.Material;

namespace ELearningSystem.Controllers.V1
{
    [Route("api/{version:apiVersion}/[controller]")]
    public class MaterialController : Controller
    {
        private readonly IMaterialService _materialService;
        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }
        [HttpPost("Material/create")]
        public async Task<IActionResult> Create([FromBody] AddMaterialDto addMaterialDto,[FromBody]IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage));
            }
            try
            {
                var result = await _materialService.AddMaterial(addMaterialDto,file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
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
