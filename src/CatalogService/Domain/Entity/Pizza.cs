using System;
using System.Collections.Generic;
using System.Linq;

namespace CatalogService.Domain.Entity;

public class Pizza
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int Price { get; set; }
    public List<PizzaTopping> PizzaToppings { get; set; } = new(); // навигац свойство

    public static Pizza Create(string name, int price, IEnumerable<PizzaTopping>? toppings = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Имя пиццы не может быть пустым", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Цена пиццы не может быть отрицательной");
        }

        return new Pizza
        {
            Id = Guid.NewGuid(),
            Name = name,
            Price = price,
            PizzaToppings = toppings?.ToList() ?? new List<PizzaTopping>()
        };
    }

}

