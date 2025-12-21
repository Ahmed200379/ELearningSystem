using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Dtos;
using Shared.Dtos.Group;
using Shared.Dtos.Material;
using System.Linq.Expressions;
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

        public async Task<GeneralResponseDto> CreateGroup(CreateGroupDto createGroupDto)
        {
            var group = new Group
            {
                Id = Guid.NewGuid().ToString(),
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
            var groupDto= groups.Select(g=> new ReadAllGroupsDto
            {
                Id= g.Id,
                Title=g.Title,
                Description=g.Description,
                CourseName = g.CourseName,
                CreatedAt = g.CreatedAt
            }).ToList();

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
            var groupsDto = groups.Select(g => new ReadAllGroupsDto
            {
                CourseName= g.CourseName,
                CreatedAt= g.CreatedAt,
                Description=g.Description,
                Id= g.Id,
                SubscriptionFee= g.SubscriptionFee,
                Title = g.Title
            }).ToList();
            var groupInPagnationDto = new ReadAllGroupsInPagnation
            {
                Groups = groupsDto,
                TotalCount = groups.Count()
            };
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Groups retrieved successfully.",
                data = groupInPagnationDto
            };
        }

        public async Task<GeneralResponseDto> GetGroupById(string groupId)
        {
            Expression<Func<Group, Object>>[] include =
            {
                g=>g.Materials
            };
            var group = await _unitOfWork.GetRepository<Group>().GetFirstOrDefault(predicate: g => g.Id == groupId,includes:include);
            var groupDto = new ReadGroupInDetails
            {
                CourseName = group.CourseName,
                CreatedAt = group.CreatedAt,
                Description = group.Description,
                Id = group.Id,
                SubscriptionFee = group.SubscriptionFee,
                Title = group.Title,
                Materials = group.Materials.Select(m => new ReadMaterialDto
                {
                    File = m.File,
                    Title = m.Title,
                    Description = m.Description,
                    AdditionDate = m.AdditionDate,
                    Id = m.Id,
                    Type = m.Type
                }).ToList()

            };
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
                data = groupDto
            };
        }

        public async Task<GeneralResponseDto> GetGroupsByCourseName(string title)
        {
            var groups=await _unitOfWork.GetRepository<Group>().GetAllAsyncs(x=>x.Title==title);
            var groupDto = groups.Select(g => new ReadAllGroupsDto
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description,
                CourseName = g.CourseName,
                CreatedAt = g.CreatedAt
            }).ToList();
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
