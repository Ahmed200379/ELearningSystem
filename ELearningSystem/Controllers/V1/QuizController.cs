using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Quiz;

namespace ELearningSystem.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllQuizzes()
        {
            var quizzes = await _quizService.GetAll();
            return Ok(quizzes);
        }
        [HttpPost]
        public async Task<IActionResult> AddQuiz([FromBody] RequestQuizDto quiz)
        {
            var addedQuiz = await _quizService.Add(quiz);
            return Ok(addedQuiz);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuiz(string id, [FromBody] RequestUpdateQuizeDto quiz)
        {
            try
            {
                var updatedQuiz = await _quizService.Update(id, quiz);
                return Ok(updatedQuiz);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteQuiz([FromQuery] string id)
        {
            try
            {
                var result = await _quizService.Delete(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
