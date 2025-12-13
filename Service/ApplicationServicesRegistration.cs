
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;

namespace Services
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IChatServices, ChatServices>();
            services.AddScoped<IGroupServices, GroupServices>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IAuthService, AuthServices>();
            // Register application services here
            return services;
        }
    }
}
