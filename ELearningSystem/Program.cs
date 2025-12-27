using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Persistence;
using Persistence.Authorization;
using Persistence.Data;
using Services;
using Shared.Helpers;
using Stripe;
using System.Security.Claims;
using System.Text;

namespace ELearningSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ===================== CORS =====================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // ===================== Controllers & SignalR =====================
            builder.Services.AddControllers();
            builder.Services.AddSignalR();

            // ===================== API Versioning =====================
            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            // ===================== Swagger =====================
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var sp = builder.Services.BuildServiceProvider();
                var provider = sp.GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(desc.GroupName, new OpenApiInfo
                    {
                        Title = "E-Learning API",
                        Version = desc.GroupName
                    });
                }

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter JWT Bearer token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(
                    JwtBearerDefaults.AuthenticationScheme,
                    securityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        securityScheme,
                        Array.Empty<string>()
                    }
                });
            });

            // ===================== JWT Authentication =====================
            var jwtOptions = builder.Configuration
                .GetSection("Jwt")
                .Get<JwtOptions>()!;

            builder.Services
                .AddAuthentication(options=>
                options.DefaultAuthenticateScheme= JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),

                        RoleClaimType = ClaimTypes.Role
                    };
                });

            // ===================== Authorization =====================
            builder.Services.AddAuthorization(options =>
            {

                options.AddPolicy("GroupAccessPolicy",
                    policy => policy.Requirements.Add(new GroupAccessRequirement()));
            });

            // ===================== DB & Identity =====================
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // ===================== App Services =====================
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices(builder.Configuration);

            // ===================== Background & Others =====================
            builder.Services.AddHostedService<BackgroundServices>();
            builder.Services.AddHttpContextAccessor();

            // ===================== Stripe =====================
            StripeConfiguration.ApiKey =
                builder.Configuration.GetSection("Stripe")["SecretKey"];

            // ===================== Mail =====================
            builder.Services.AddMailKit(config =>
            {
                config.UseMailKit(
                    builder.Configuration
                        .GetSection("EmailSetting")
                        .Get<MailKitOptions>());
            });

            var app = builder.Build();

            // ===================== Seed Roles =====================
            using (var scope = app.Services.CreateScope())
            {
                var roleManager =
                    scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roles = { "Admin", "Student", "Teacher", "SuperAdmin" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            // ===================== Middleware =====================
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers().RequireAuthorization(new AuthorizeAttribute() { AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme });

            app.MapHub<ChatHub>("/chatHub");

            app.Run();
        }
    }
}