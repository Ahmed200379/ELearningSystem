
using Domain.Entities;
using Shared.Dtos.Answer;

namespace Services.Abstractions
{
    public interface IAnswerService
    {
        Task<Answer> CreateAnswer(RequestAnswerDto answerDto);
        Task<Answer> UpdateAnswer(string id, RequestAnswerDto answerDto);
        Task<bool> DeleteAnswer(string id);
        Task<ResponseAnswerDto> GetAnswerById(string id);
        Task<IEnumerable<ResponseAnswerDto>> GetAllAnswers();
    }
}
