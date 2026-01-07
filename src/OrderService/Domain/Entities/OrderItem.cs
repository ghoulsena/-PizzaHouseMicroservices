using OrderService.Domain.Entities;

namespace OrderService.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; set; } 
    public Guid PzzaId { get; set; }
    public int Quantity { get; set; }//колво
    public int Price { get; set; }
    

     public Guid OrderId { get; set; } 
    public Order? Order { get; set; }
}

