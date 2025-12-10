
using Domain.Entities;
using Shared.Dtos.Question;

namespace Services.Abstractions
{
    public interface IQuestionService
    {
        Task<IEnumerable<ResponseQuestionDto>> GetAllQuestionsAsync();
        Task<ResponseQuestionDto> GetQuestionByIdAsync(string id);
        Task<Question> CreateQuestionAsync(RequestQuestionDto question);
        Task<Question> UpdateQuestionAsync(string id, RequestQuestionDto question);
        Task<string> DeleteQuestionAsync(string id);
    }
}
