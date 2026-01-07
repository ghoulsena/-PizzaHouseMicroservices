using AutoMapper;
using DeliveryService.Application.DTOs;
using DeliveryService.Domian.Entity;

namespace DeliveryService.Application.Mappings
{
    public class CourierProfile : Profile
    {
        public CourierProfile()
        {
            CreateMap<Courier, CourierDto>();
        }
    }
}
