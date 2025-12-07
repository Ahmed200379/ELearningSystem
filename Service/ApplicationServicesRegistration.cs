using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
namespace Services
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register application services here
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IAuthService, AuthServices>();
            services.AddScoped<IGroupServices, GroupServices>();
            services.AddScoped<IMaterialService, MaterialService>();
           // services.AddScoped<IImageStorage, ImageStorage>();

            return services;
        }
    }
}
