
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Repos;
using Persistence.Storage;
using NETCore.MailKit.Core;

namespace Persistence
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddMemoryCache();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtRepo, JwtRepo>();
            services.AddScoped<IUserGroupRepo, UserGroupRepo>();
            services.AddScoped<IImageStorage, ImageStorage>();
            return services;
        }
    }

}
