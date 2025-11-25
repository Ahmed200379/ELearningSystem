using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;
namespace Persistence.Repos
{
    public class UserGroupRepo : IUserGroupRepo
    {
        private readonly AppDbContext _context;
        public UserGroupRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddUserGroup(UserGroup userGroup)
        {
            await _context.UserGroups.AddAsync(userGroup);
        }

        public Task<bool> IsUserInGroup(string userId, string groupId)
        {
            throw new NotImplementedException();
        }
    }
}
