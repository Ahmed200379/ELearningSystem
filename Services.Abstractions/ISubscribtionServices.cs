using Shared.Dtos;
using Shared.Dtos.Group;

namespace Services.Abstractions
{
    public interface ISubscribtionServices
    {
        public Task<GeneralResponseDto> AddStudentToGroup(AddStudentDto addStudentToGroupDto);

    }
}
