using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos;
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
            return Ok(new GeneralResponseDto
            {
                statusCode = StatusCodes.Status200OK,
                message = "Answers retrieved successfully",
                data = answers,
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnswerById(string id)
        {
            try
            {
                var answer = await _answerService.GetAnswerById(id);
                return Ok(new GeneralResponseDto
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Answer retrieved successfully",
                    data = answer,
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new GeneralResponseDto
                {
                    statusCode = StatusCodes.Status404NotFound,
                    message = ex.Message,
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAnswer([FromBody] RequestAnswerDto answerDto)
        {
            try
            {
                var createdAnswer = await _answerService.CreateAnswer(answerDto);
                return Ok(new GeneralResponseDto
                {
                    statusCode = StatusCodes.Status201Created,
                    message = "Answer created successfully",
                    data = createdAnswer,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralResponseDto
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = ex.Message,
                });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnswer(string id, [FromBody] RequestAnswerDto answerDto)
        {
            try
            {
                var updatedAnswer = await _answerService.UpdateAnswer(id, answerDto);
                return Ok(new GeneralResponseDto
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Answer updated successfully",
                    data = updatedAnswer,
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new GeneralResponseDto
                {
                    statusCode = StatusCodes.Status404NotFound,
                    message = ex.Message,
                });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAnswer([FromQuery] string id)
        {
            try
            {
                var result = await _answerService.DeleteAnswer(id);
                return Ok(new GeneralResponseDto
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Answer deleted successfully",
                    data = result,
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new GeneralResponseDto
                {
                    statusCode = StatusCodes.Status404NotFound,
                    message = ex.Message,
                });
            }
        }
    }
}
