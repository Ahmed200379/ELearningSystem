using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Domain.Interfaces
{
    public interface IJwtRepo
    {
        public Task<JwtSecurityToken> GenerateToken(User user);
    }
}
