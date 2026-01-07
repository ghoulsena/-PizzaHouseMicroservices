namespace OrderService.Application.DTOs;

public class Topping
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<string> Pizzas { get; set; } = new();
}
