using Shared.Dtos;
using Shared.Dtos.Subscribe;

namespace Services.Abstractions
{
    public interface ISubscribtionServices
    {
        public Task<GeneralResponseDto> AddStudentToGroup(AddStudentDto addStudentToGroupDto);
        public Task<GeneralResponseDto> UpdateSubscribeManually(UpdateSubscribeDto updateSubscribeDto);
        public Task<GeneralResponseDto> GetAllSubscribtionGroupsForStudent(string studentId);
        public Task<GeneralResponseDto> CheckUserSubscriptionStatus(string userId, string groupId);
        public Task<GeneralResponseDto> GetAllStudentsForSpecificGroup(string groupId);
    }
}
