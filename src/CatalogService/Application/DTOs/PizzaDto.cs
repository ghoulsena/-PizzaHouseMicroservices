using CatalogService.Domain.Entity;

namespace CatalogService.Application.DTOs
{
    public class PizzaDTO
    {
        public string? Name { get; set; }
        public int Price { get; set; }
        public List<PizzaTopping> PizzaToppings { get; set; } = new(); 

    }
}

