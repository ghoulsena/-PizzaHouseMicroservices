using JwtOptioncs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UserService.Application.Services;
using UserService.Application.DomainEventHandlers;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Repository;
using UserService.Infrastructure.UserDbContext;
using UserService.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class MyConfigServiceCollectionExtensions
{
    public static IServiceCollection AddMyDependencyGrop(this IServiceCollection services)
    {
        services.AddScoped<RoleService>();
        services.AddSingleton<IKafkaProducer, KafkaProducer>();
        services.AddScoped<UserAppService>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthProvider, JwtAuthProvider>();

        //добавляет медиар, сканирует указанные сборки на наличие handlkes
        //указываем медиатр искать классы обработчкии в той же сборке где и наш класс UserRegisteredDomainEventHandler

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(typeof(UserRegisteredDomainEventHandler).Assembly);
        });

        return services;
    }


    public static IServiceCollection AddMyAddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        var jwt = configuration.GetSection(JwtOptions.Jwt).Get<JwtOptions>();
        if(jwt == null)
        {
            throw new InvalidOperationException("Jwt содержит значение null");
        }
        opts.TokenValidationParameters = new TokenValidationParameters
        {


            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,

            ValidateAudience = true,
            ValidAudience = jwt.Audience,

            ValidateLifetime = true,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            ValidateIssuerSigningKey = true,
        };
    });
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Jwt));
       
        return services;
    }

    public static IServiceCollection AddMySwagger(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserService", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Введите токен так: Bearer {ваш токен}"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
        });

        return services;
        
       
                
    }

    public static void AddMyDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbCont>(options =>
           options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    }
