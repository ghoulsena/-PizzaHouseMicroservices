using OrderService.Domain.Entities;
using OrderService.Application.DTOs;

namespace OrderService.Mapper
{
    public static class OrderMapper
    {
        public static Order ToDomain(this OrderDto dto, Guid userId)
        {
            return new Order
            {
                UserId = userId,
                Status = dto.Status,
                Items = dto.Items.Select(i => new OrderItem
                {
                    PzzaId = i.PzzaId,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
        

        public static OrderDto ToDto(this Order order)
        {
            return new OrderDto
            {
                OrderId = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    PzzaId = i.PzzaId,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
    }
}
