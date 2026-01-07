using DeliveryService.Application.Interface;
using DeliveryService.Application.Service;
using DeliveryService.Http;
using DeliveryService.Infrastructure.Repositories;

namespace DeliveryService.Extensions
{
    public static class ConfigServiceColl
    {
        public static IServiceCollection AddMyDependencyGroup(
         this IServiceCollection services)
        {
            services.AddScoped<ICourierRepository, CourierRepository>();
            services.AddScoped<IDeliveryTaskRepository, DeliveryTaskRepository>();
            services.AddScoped<CourierService>();
            services.AddScoped<DeliveryTaskService>();
            services.AddScoped<DeliveryTaskValidatorService>();
            services.AddScoped<IDeliveryServiceClient, DeliveryServiceClient>();




            return services;
        }
 
    }
}
