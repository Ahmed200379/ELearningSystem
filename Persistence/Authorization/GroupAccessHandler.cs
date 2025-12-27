using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Authorization
{
    public class GroupAccessHandler :AuthorizationHandler<GroupAccessRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        public  GroupAccessHandler(IHttpContextAccessor httpContextAccessor,IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        protected override  async Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupAccessRequirement requirement)
        {
            var userId =
              context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? context.User.FindFirst("sub")?.Value;
            var role = context.User?.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }
            if (role == "Admin" || role == "SuperAdmin")
            {
                context.Succeed(requirement);
                return;
            }
            var httpContext = _httpContextAccessor.HttpContext;
            var groupId=httpContext?.Request.RouteValues["groupId"]?.ToString();
            if (string.IsNullOrEmpty(groupId))
            {
                return;
            }
            var member = await _unitOfWork.GetRepository<UserGroup>().GetFirstOrDefault(g => g.GroupId == groupId && g.UserId == userId && g.IsActive);
            if (member ==null)
            {
                 return;
            }
            if (member.RoleInGroup == Role.Admin||member.RoleInGroup == Role.Teacher)
            {
                context.Succeed(requirement);
            }

        }
    }
}
