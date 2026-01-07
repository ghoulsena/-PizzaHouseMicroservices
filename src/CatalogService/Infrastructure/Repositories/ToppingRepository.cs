using CatalogService.Infrastructure.CatalogDbcontext;
using CatalogService.Application.Interface;
using Microsoft.EntityFrameworkCore;
using CatalogService.Domain.Entity;

namespace CatalogService.Infrastructure.Repositories;

public class ToppingRepository : IToppingRepository
{
    private readonly CatalogDb _catalogDb;
    public ToppingRepository(CatalogDb catalogDb)
    {
        _catalogDb = catalogDb;
    }
    public async Task<string?> GetToppingNameAsync(Guid id) => await _catalogDb.Toppings.Where(p => p.Id == id).Select(p => p.Name).FirstOrDefaultAsync();
    public async Task<List<Topping>> GetAllAsync() =>  await _catalogDb.Toppings.ToListAsync();

    public async Task<Topping?> GetByIdAsync(Guid id) => await _catalogDb.Toppings.FindAsync(id);
    public async Task DeleteAsync(Guid id)
    {
        var Topping = await _catalogDb.Toppings.FindAsync(id);
        if (Topping != null)
        {
            _catalogDb.Toppings.Remove(Topping);
            await _catalogDb.SaveChangesAsync();
        }
    }
    public async Task AddAsync(Topping topping)
    {
        await _catalogDb.Toppings.AddAsync(topping);
        await _catalogDb.SaveChangesAsync();
    }
    public async Task UpdateAsync(Topping topping)
    {
        _catalogDb.Toppings.Update(topping);
        await _catalogDb.SaveChangesAsync();
    }

    public async Task<Pizza?> GetPizzaWithToppingsAsync(Guid pizzaId)
    {
        return await _catalogDb.Pizzas
            .Include(p => p.PizzaToppings)       // подгружаем связь Pizza-Topping
            .ThenInclude(pt => pt.Topping)       // подгружаем сами топпинги
            .FirstOrDefaultAsync(p => p.Id == pizzaId);
    }
}