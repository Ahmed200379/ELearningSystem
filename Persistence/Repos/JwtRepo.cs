using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Dtos.Auth;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repos
{
    public class JwtRepo : IJwtRepo
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        public JwtRepo(IConfiguration configuration,UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<JwtSecurityToken> GenerateToken(User user)
        {
            var jwtOptions=_configuration.GetSection("Jwt").Get<JwtOptions>();
            var symmetrickey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SigningKey));
            var signingCredentials = new SigningCredentials(symmetrickey,SecurityAlgorithms.HmacSha256);
            var userClaims= await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName!),
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("userid",user.Id)
            }.Union(roleClaims)
            .Union(userClaims);
            var token = new JwtSecurityToken(
                issuer: jwtOptions.Isusser,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtOptions.LifeTime),
                signingCredentials: signingCredentials
                );
            return token;
        }
    }
}
