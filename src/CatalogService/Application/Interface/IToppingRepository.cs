
using CatalogService.Domain.Entity;

namespace CatalogService.Application.Interface;

public interface IToppingRepository
{

    Task<string?> GetToppingNameAsync(Guid id);
    Task<List<Topping>> GetAllAsync();
    Task<Topping?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task AddAsync(Topping  topping);                  
    Task UpdateAsync(Topping  topping);
    Task<Pizza?> GetPizzaWithToppingsAsync(Guid pizzaId);
}