using Shared.Dtos;
using Shared.Dtos.role;
using Shared.Dtos.RoleManagement;
using Shared.Enums;

namespace Services.Abstractions
{
    public interface IRoleManagementService
    {
        public Task<GeneralResponseDto> AddRole(AddRoleDto addRoleDto);
        public Task<GeneralResponseDto> RemoveRole(RemoveRoleDto removeRoleDto);
        public Task<GeneralResponseDto> GetAllUsersWithSpecificRole(Role role);

    }
}
