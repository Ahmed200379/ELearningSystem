using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Shared.Dtos;
using Shared.Dtos.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                ExpirationDate = DateTime.UtcNow.AddMonths(1)
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
    }
}
