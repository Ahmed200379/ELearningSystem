using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Answer;

namespace ELearningSystem.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAnswers()
        {
            var answers = await _answerService.GetAllAnswers();
            return Ok(answers);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnswerById(string id)
        {
            try
            {
                var answer = await _answerService.GetAnswerById(id);
                return Ok(answer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAnswer([FromBody] RequestAnswerDto answerDto)
        {
            var createdAnswer = await _answerService.CreateAnswer(answerDto);
            return Ok(createdAnswer);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnswer(string id, [FromBody] RequestAnswerDto answerDto)
        {
            try
            {
                var updatedAnswer = await _answerService.UpdateAnswer(id, answerDto);
                return Ok(updatedAnswer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAnswer([FromQuery] string id)
        {
            try
            {
                var result = await _answerService.DeleteAnswer(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
