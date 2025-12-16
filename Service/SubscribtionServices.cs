using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Shared.Dtos;
using Shared.Dtos.Subscribe;
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
            user.ExpirationDate.AddMonths(updateSubscribeDto.DurationInMonths);
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
    }
}
