
using OrderService.Domain.Enums;


namespace OrderService.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public int TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Order() { }


    public static Order CreateNew(Guid userId, List<OrderItem> items)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Items = items,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending
        };

        order.CalculateTotalPrice();
        return order;
    }

    private void CalculateTotalPrice()
    {
        TotalPrice = Items.Sum(i => i.Price * i.Quantity);
    }
}


