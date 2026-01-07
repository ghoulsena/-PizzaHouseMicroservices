
using CatalogService.Domain.Entity;

namespace PizzaMenu;

public class Menu
{
    public Guid Id { get; set; }

    public List<Pizza> PizzaMenu { get; set; } = new();

    public static Menu Create(List<Pizza> PizzaMenu)
    {
        return  new Menu
        {
            Id = Guid.NewGuid(),
            PizzaMenu = PizzaMenu
        };


    }
}