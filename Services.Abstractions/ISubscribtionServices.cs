using Shared.Dtos;
using Shared.Dtos.Subscribe;

namespace Services.Abstractions
{
    public interface ISubscribtionServices
    {
        public Task<GeneralResponseDto> AddStudentToGroup(AddStudentDto addStudentToGroupDto);
        public Task<GeneralResponseDto> UpdateSubscribeManually()
    }
}
