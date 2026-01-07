using OrderService.Http;
using OrderService.Application.Interface;
using JwtOptioncs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;
using OrderService.Application.Service;
using OrderService.Presentation.OrderOwnerRequirement;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

public static class MyConfigServiceCollectionExtensions
{
    public static IServiceCollection AddMyDependencyGroup(
         this IServiceCollection services)
    {

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<OrderServicee>();
        services.AddScoped<ICatalogHttpClient, CatalogHttpClient>();
        services.AddScoped<CatalogHttpClient>();
        services.AddControllers();


        services.AddHttpClient<CatalogHttpClient>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:5001");
        });

        

        return services;
    }
    
    public static IServiceCollection AddMySwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderService", Version = "v1" });

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

  
    public static IServiceCollection AddMyAddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt =>
        {
            var jwt = configuration.GetSection(JwtOptions.Jwt).Get<JwtOptions>();
            if (jwt == null)
            {
                throw new ArgumentException("Jwt содержит значение null");
            }
            opt.TokenValidationParameters = new TokenValidationParameters
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

        services.AddAuthorization(options =>
        {
            options.AddPolicy("OrderOwner", policy =>
                policy.Requirements.Add(new OrderOwnerRequirement()));//регистрируем политику
                                                                      //Политика содержит список требований (IAuthorizationRequirement).
                                                                      //OrderOwnerRequiremen наследуется от  (IAuthorizationRequirement). поэтому добавляет ее,
                                                                      //OrderOwnerRequiremen  это наше требование

        });

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Jwt));
        return services;
    }
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<OrderDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }
    

}