
using CatalogService.Application.DTOs;
using CatalogService.Domain.Entity;

namespace CatalogService.Application.Interface;

public interface IPizzaRepository
{
    Task<string?> GetPizzaNameByIdAsync(Guid id);
    Task<List<Pizza>> GetAllAsync();
    Task<int> GetPizzaPriceAsync(Guid id);
    Task<Pizza?> GetByIdAsync(Guid id);

    Task<Pizza?> AddToppingsToPizzaAsync(Guid pizzaId, params Guid[] toppingIds);
    Task<Pizza?> RemoveToppingFromPizzaAsync(Guid pizzaId, Guid toppingId);
    Task<IEnumerable<PizzaToppingDto>> GetToppingsForPizzaAsync(Guid pizzaId);
    Task DeleteAsync(Guid id);
    Task AddAsync(Pizza  pizza);                  
    Task UpdateAsync(Pizza pizza);

    

}
