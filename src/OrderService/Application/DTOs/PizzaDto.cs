
using OrderService.Application.DTOs;

namespace OrderService.Application.DTOs;

public class DTOPizza
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
    public List<Topping> Topping { get; set; } = new();

}

