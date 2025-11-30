using Domain.Interfaces;
using Persistence.Repos;
using Services;

namespace ELearningSystem.Properties
{
    public class ApplicationServices
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthServices>();
            services.AddScoped<IGroupServices, GroupServices>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtRepo, JwtRepo>();
        }
    }
}
