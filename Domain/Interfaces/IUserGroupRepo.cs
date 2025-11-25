using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserGroupRepo
    {
        Task<bool> IsUserInGroup(string userId, string groupId);
        Task AddUserGroup(UserGroup userGroup);
    }
}
