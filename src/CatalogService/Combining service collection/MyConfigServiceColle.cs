

using CatalogService.Application.Interface;
using CatalogService.Application.Service;
using CatalogService.Infrastructure.CatalogDbcontext;
using CatalogService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Microsoft.Extensions.DependencyInjection;

public static class MyConfigServiceCollectionExtensions
{
    public static IServiceCollection AddMyDependencyGroup(this IServiceCollection services)
    {

        services.AddScoped<PizzaService>();
        services.AddScoped<ToppingService>();

        services.AddScoped<IPizzaRepository, PizzaRepository>();

        services.AddScoped<IToppingRepository, ToppingRepository>();

        return services;
        
    }
    public static IServiceCollection AddMySwagger(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog Service", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "¬ведите токен так: Bearer {ваш токен}"
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
        services.AddEndpointsApiExplorer();
        services.AddControllers();
        return services;



    }
    public static void AddMyDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDb>(options =>
           options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}