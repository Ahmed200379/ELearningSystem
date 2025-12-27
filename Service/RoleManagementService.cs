using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Shared.Dtos;
using Shared.Dtos.role;
using Shared.Dtos.RoleManagement;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RoleManagementService: IRoleManagementService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public RoleManagementService(UserManager<User> userManager,IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<GeneralResponseDto> AddRole(AddRoleDto addRoleDto)
        {
            var user = await _userManager.FindByEmailAsync(addRoleDto.Email);
            if (user==null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "user is not found.",
                };
            }
            var result= await _userManager.AddToRoleAsync(user, addRoleDto.role.ToString());
            if (!result.Succeeded)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = true,
                    message = "Faild to add role.",
                };
            }
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "role added successfully.",
            };

        }

        public async Task<GeneralResponseDto> GetAllUsersWithSpecificRole(Role role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role.ToString());
            if (users==null)
            {
                 return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "There is no users with this role.",
                };
            }
            var userDtos = users.Select(async u => new ReadUsersDto
            {
                Email=u.Email,
                Id=u.Id,
                Name=u.FirstName+" "+u.SecondName,
            }
                );
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = $"Users retrieved successfully with {role.ToString()} role.",
                data=userDtos
            };

        }

        public async Task<GeneralResponseDto> RemoveRole(RemoveRoleDto removeRoleDto)
        {
            var user = await _userManager.FindByEmailAsync(removeRoleDto.Email);
            if (user ==null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "user is not found.",
                };
            }
            var result = await _userManager.RemoveFromRoleAsync(user, removeRoleDto.role.ToString());
            if (!result.Succeeded)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = true,
                    message = "Faild to remove role.",
                };
            }
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "role deleted successfully.",
            };
        }
    }
}
