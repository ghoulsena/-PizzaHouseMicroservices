using System;
using System.Collections.Generic;

namespace CatalogService.Domain.Entity;

public class Topping
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int ExtraPrice { get; set; }
     public List<PizzaTopping> PizzaToppings { get; set; } = new();


    public static Topping Create(string? name, int extraPrice, List<PizzaTopping> pizzatoppings)
    {
        return new Topping
        {
            Id = Guid.NewGuid(),
            Name = name,
            ExtraPrice = extraPrice,
            PizzaToppings = pizzatoppings
        };
    }


}
