using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Dtos;
using Shared.Dtos.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class GroupServices : IGroupServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        public GroupServices(IUnitOfWork unitOfWork,UserManager<User> userManager)
        { 
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<GeneralResponseDto> AddStudentToGroup(AddStudentDto addStudentToGroupDto)
        {
            var student= await _userManager.FindByEmailAsync(addStudentToGroupDto.Email);
            if (student == null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "User not found with this email."
                };
            }
            var joinstudent = new UserGroup()
            {
                GroupId = addStudentToGroupDto.GroupId,
                UserId = student.Id,
            };
            await _unitOfWork.GetRepository<UserGroup>().AddAsync(joinstudent);
            var result = await _unitOfWork.SaveChanges();
            if (result == 0)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Failed to add student to group."
                };
            }
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Student added to group successfully."
            };
        }

        public async Task<GeneralResponseDto> CreateGroup(CreateGroupDto createGroupDto)
        {
            var group = new Group
            {
                Title = createGroupDto.Title,
                Description = createGroupDto.Description,
                CourseName = createGroupDto.NameOfCourse,
                CreatedAt = DateTime.Now
            };
           await _unitOfWork.GetRepository<Group>().AddAsync(group);
           var result= await _unitOfWork.SaveChanges();
            if (result==0)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Failed to create group."
                };
            }
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Group created successfully."
            };
        }

        public async Task<GeneralResponseDto> DeleteGroup(string groupId)
        {
            var group = await _unitOfWork.GetRepository<Group>().GetByIdAsync( groupId);
            if (group == null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Group not found."
                };
            }
            _unitOfWork.GetRepository<Group>().Delete(group);
            var result = await _unitOfWork.SaveChanges();
            if (result == 0)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Failed to delete group."
                };
            }
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Group deleted successfully."
            };
        }

        public async Task<GeneralResponseDto> GetAllGroups()
        {
            var groups = await _unitOfWork.GetRepository<Group>().GetAllAsync();
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Groups retrieved successfully.",
                data = groups
            };
        }

        public async Task<GeneralResponseDto> GetAllInPagination(int pageNumber, int pageSize)
        {
            var groups = await _unitOfWork.GetRepository<Group>().GetAllAsyncs(PageNumber: pageNumber, PageSize: pageSize);
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Groups retrieved successfully.",
                data = groups
            };
        }

        public async Task<GeneralResponseDto> GetGroupById(string groupId)
        {
            var group = await _unitOfWork.GetRepository<Group>().GetByIdAsync(groupId);
            if (group == null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Group not found."
                };
            }
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Group retrieved successfully.",
                data = group
            };
        }

        public async Task<GeneralResponseDto> GetGroupsByCourseName(string title)
        {
            var groups=await _unitOfWork.GetRepository<Group>().GetFirstOrDefault(x=>x.Title==title);
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Groups retrieved successfully.",
                data = groups
            };
        }

        public async Task<GeneralResponseDto> UpdateGroup(UpdateGroupDto updateGroupDto)
        {
            var group = await  _unitOfWork.GetRepository<Group>().GetByIdAsync(updateGroupDto.Id);
            if (group == null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Group not found."
                };
            }
            group.Title = updateGroupDto.Title;
            group.Description = updateGroupDto.Description;
            group.CourseName = updateGroupDto.NameOfCourse;
             _unitOfWork.GetRepository<Group>().Update(group);
            var result = await _unitOfWork.SaveChanges();
            if (result == 0)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Failed to update group."
                };
            }
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Group updated successfully."
            };
        }
    }
}
