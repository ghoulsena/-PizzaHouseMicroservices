using AutoMapper;
using DeliveryService.Application.DTOs;
using DeliveryService.Domian.Entity;

namespace DeliveryService.Application.Mappings
{
    public class DeliveryProfile:Profile
    {
        public DeliveryProfile()
        {
            CreateMap<DeliveryTask, DeliveryTaskDto>();
        }
    }
}
