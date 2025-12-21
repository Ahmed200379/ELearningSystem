using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Shared.Dtos;
using Shared.Dtos.Group;
using Shared.Dtos.Subscribe;
using Shared.Enums;
using System.Linq.Expressions;
namespace Services
{
    public class SubscribtionServices:ISubscribtionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        public SubscribtionServices(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<GeneralResponseDto> AddStudentToGroup(AddStudentDto addStudentToGroupDto)
        {
            var student = await _userManager.FindByEmailAsync(addStudentToGroupDto.Email);
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
                SupscriptionTime = DateTime.UtcNow,
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddMonths(addStudentToGroupDto.DurationInMonths)
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

        public async Task<GeneralResponseDto> UpdateSubscribeManually(UpdateSubscribeDto updateSubscribeDto)
        {
            var user= await _unitOfWork.GetRepository<UserGroup>().GetFirstOrDefault(u=>u.GroupId==updateSubscribeDto.GroupId && u.UserId==updateSubscribeDto.UserId); 
            if (user == null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "User not found with this email."
                };
            }
            user.ExpirationDate=DateTime.UtcNow.AddMonths(updateSubscribeDto.DurationInMonths);
            user.IsActive = true;
            _unitOfWork.GetRepository<UserGroup>().Update(user);
            var result= await _unitOfWork.SaveChanges();
            if (result == 0)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Failed to update subscribtion."
                };
            }
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Subscribe updated successfully."
            };
        }
        public async Task<GeneralResponseDto> GetAllSubscribtionGroupsForStudent(string studentId)
        {
            Expression<Func<UserGroup, Object>>[] include= {g=>g.Group};
            var groups = await _unitOfWork.GetRepository<UserGroup>().GetAllAsyncs(includes:include,predicate: ug => ug.UserId == studentId && ug.RoleInGroup == Role.Student);
            var groupDto = groups.Select(g => new ReadSubscribeDto
            {
              GroupId = g.GroupId,
              Title= g.Group.Title,
              IsActive= g.IsActive
            }).ToList();
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Groups retrieved successfully.",
                data = groupDto
            };
        }

        public async Task<GeneralResponseDto> CheckUserSubscriptionStatus(string userId, string groupId)
        {
            var userSubscription = await _unitOfWork.GetRepository<UserGroup>().GetFirstOrDefault(ug => ug.UserId == userId && ug.GroupId == groupId);
            if (userSubscription == null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "No subscription found for the user in the specified group."
                };
            }
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Subscription found.",
                data = new 
                {
                    userSubscription.IsActive,
                    userSubscription.ExpirationDate
                }
            };
        }

        public async Task<GeneralResponseDto> GetAllStudentsForSpecificGroup(string groupId)
        {
            var include = new Expression<Func<UserGroup, Object>>[] { g => g.User };
                var students = await _unitOfWork.GetRepository<UserGroup>().GetAllAsyncs(includes: include, predicate: ug => ug.GroupId == groupId && ug.RoleInGroup == Role.Student && ug.IsActive==true);
                if (students == null || !students.Any())
                {
                    return new GeneralResponseDto
                    {
                        IsSuccess = false,
                        message = "No students found for the specified group."
                    };
            }
            var studentDtos = students.Select(s => new ReadSubscribtionStudents
                {
                   Students= students.Select( u=> new ReadStudentDto
                   {
                      StudentId= u.UserId,
                      FullName= u.User.FirstName+" "+ u.User.SecondName,
                      Email= u.User.Email
                   }).ToList()
                }).ToList();
                return new GeneralResponseDto
                {
                    IsSuccess = true,
                    message = "Students retrieved successfully.",
                    data = studentDtos
                };
        }
    }
}