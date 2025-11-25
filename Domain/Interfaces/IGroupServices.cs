using Shared.Dtos;
using Shared.Dtos.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGroupServices
    {
        public Task<GeneralResponseDto> CreateGroup(CreateGroupDto createGroupDto);
        public Task<GeneralResponseDto> UpdateGroup(UpdateGroupDto updateGroupDto);
        public Task<GeneralResponseDto> DeleteGroup(string groupId);
        public Task<GeneralResponseDto> GetAllGroups();
        public Task<GeneralResponseDto> GetGroupById(string groupId);
        public Task<GeneralResponseDto> GetGroupsByCourseName(string title);
        public Task<GeneralResponseDto> GetAllInPagination(int pageNumber, int pageSize);
        public Task<GeneralResponseDto> AddStudentToGroup(AddStudentDto addStudentToGroupDto);
    }
}
