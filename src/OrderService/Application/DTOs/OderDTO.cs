using OrderService.Domain.Entities;
using OrderService.Domain.Enums;

namespace OrderService.Application.DTOs;

public class OrderDto
{
    public Guid OrderId { get; set; }          
    public Guid UserId { get; set; }       
    public int TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public List<OrderItemDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
