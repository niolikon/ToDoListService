using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ToDoListService.Framework.Security.Authentication;
using ToDoListService.Framework.Security.Jwt;
using ToDoListService.Domain.Entities;
using ToDoListService.Domain.Mappers;
using ToDoListService.Domain.Repositories;
using ToDoListService.Domain.Services;
using ToDoListService.Infrastructure.Repositories;
using ToDoListService.Framework.Utils.Swagger;

namespace ToDoListService.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        #region Mappers
        services.AddTransient<IUserMapper, UserMapper>();
        services.AddTransient<IToDoMapper, ToDoMapper>();
        #endregion

        #region Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IToDoService, ToDoService>();
        #endregion

        #region Repositories
        services.AddScoped<IToDoRepository, ToDoRepository>();
        #endregion

        #region JWT
        JwtSystemConfig tokenConfig = new JwtSystemConfig(configuration.GetSection("JwtConfig"));
        services.AddSingleton<JwtSystemConfig>(tokenConfig.Verified());
        services.AddScoped<IJwtTokenFactory, JwtTokenFactory>();
        #endregion

        #region Authentication
        services.AddTransient<IAuthenticatedUserFactory, AuthenticatedUserFactory>();
        #endregion

        return services;
    }

    public static IServiceCollection ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }

    public static IServiceCollection AddAuthenticationWithJwtBearerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        JwtSystemConfig tokenConfig = new JwtSystemConfig(configuration.GetSection("JwtConfig"));

        // Configure Identity Core
        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // Configures authentication service so as to use JWT Bearer
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        // JWT Bearer handler configuration
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = tokenConfig.Issuer,
                ValidAudience = tokenConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.Secret))
            };
        });

        return services;
    }

    public static IServiceCollection ConfigureSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<CustomResponseContentTypeFilter>();

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ToDoListService API",
                Version = "0.0.1",
                Description = "ToDo List Management API",
            });

            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme.Example: \"Authorization: Bearer {token}\"",

                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            options.AddSecurityDefinition("Bearer", securityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement 
            {
                {
                    securityScheme,
                    new[] { "Bearer" }
                }
            });
        });

        return services;
    }
}
