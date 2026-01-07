using CatalogService.Domain.Entity;


namespace CatalogService.Domain.Entity;

public class PizzaTopping
    {
        //внешний ключ
        public Guid PizzaId { get; set; }
        //нав-ое свойство
        public Pizza Pizza { get; set; } = null!;

        public Guid ToppingId { get; set; }
        public Topping Topping { get; set; } = null!;
    }
