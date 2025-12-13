using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos;
using Shared.Dtos.Question;

namespace ELearningSystem.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await _questionService.GetAllQuestionsAsync();
            return Ok(new GeneralResponseDto
            {
                statusCode = StatusCodes.Status200OK,
                message = "Questions retrieved successfully",
                data = questions,
            });
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetQuestionById(string id)
        {
            try
            {
                var question = await _questionService.GetQuestionByIdAsync(id);
                return Ok(new GeneralResponseDto
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Question retrieved successfully",
                    data = question,
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
        public async Task<IActionResult> CreateQuestion([FromBody] RequestQuestionDto question)
        {
            var createdQuestion = await _questionService.CreateQuestionAsync(question);
            return Ok(new GeneralResponseDto
            {
                statusCode = StatusCodes.Status200OK,
                message = "Question created successfully",
                data = createdQuestion,
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(string id, [FromBody]RequestQuestionDto question)
        {
            try
            {
                var updatedQuestion = await _questionService.UpdateQuestionAsync(id, question);
                return Ok(new GeneralResponseDto
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Question updated successfully",
                    data = updatedQuestion,
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
        public async Task<IActionResult> DeleteQuestion([FromQuery] string id)
        {
            try
            {
                var result = await _questionService.DeleteQuestionAsync(id);
                return Ok(new GeneralResponseDto
                {
                    statusCode = StatusCodes.Status200OK,
                    message = "Question deleted successfully",
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
