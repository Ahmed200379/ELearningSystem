
using Domain.Entities;
using Shared.Dtos.Quiz;

namespace Services.Abstractions
{
    public interface IQuizService
    {
        Task<IEnumerable<ResponseQuizDto>> GetAll();
        Task<Quiz> Add(RequestQuizDto requet);
        Task<Quiz> Update(string id, RequestUpdateQuizeDto request);
        Task<string> Delete(string id);
    }
}
