using CatalogService.Infrastructure.CatalogDbcontext;
using CatalogService.Application.DTOs;
using CatalogService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using CatalogService.Application.Interface;

namespace CatalogService.Infrastructure.Repositories;

public class PizzaRepository: IPizzaRepository
{
    private readonly CatalogDb _catalogDb;
    public PizzaRepository(CatalogDb catalogDb)
    {
        _catalogDb = catalogDb;
    }
    public async Task<string?> GetPizzaNameByIdAsync(Guid id) => await _catalogDb.Pizzas.Where(p => p.Id == id).Select(p => p.Name).FirstOrDefaultAsync();
    public async Task<List<Pizza>> GetAllAsync() => await _catalogDb.Pizzas.ToListAsync();
    public async Task<int> GetPizzaPriceAsync(Guid id) => await _catalogDb.Pizzas.Where(p => p.Id == id).Select(p => p.Price).FirstOrDefaultAsync();
    public async Task<Pizza?> GetByIdAsync(Guid id) => await _catalogDb.Pizzas.FindAsync(id);

    //Переписал метод, в пицца Topping нужно было передавать сами обьект, а не них Id
    //Тк EF отслеживает сущности а не сами чисто
    //    EF видит PizzaTopping как новую сущность, но у неё есть внешние ключи(PizzaId и ToppingId),
    //    которые ссылаются на другие таблицы(Pizzas и Toppings).

    //Если EF не отслеживает объекты Pizza и Topping с этими Id, то при SaveChanges
    //он не может быть уверен, что эти Id реально существуют в базе — и выдает ошибку 
    public async Task<Pizza?> AddToppingsToPizzaAsync(Guid pizzaId, params Guid[] toppingIds)
    {
        // получаем пиццу вместе с уже существующими связями
        var pizza = await _catalogDb.Pizzas
            .Include(p => p.PizzaToppings)
            .FirstOrDefaultAsync(p => p.Id == pizzaId);
        if (pizza == null)
            return null;

        foreach (var toppingId in toppingIds)
        {
            // загружаем топпинг из БД
            var topping = await _catalogDb.Toppings.FindAsync(toppingId);
            if (topping == null) continue; // или выбросить исключение

            pizza.PizzaToppings.Add(new PizzaTopping
            {
                Pizza = pizza,       
                Topping = topping   
            });
        }

        await _catalogDb.SaveChangesAsync();
        return pizza;
    }

   
    public async Task<Pizza?> RemoveToppingFromPizzaAsync(Guid pizzaId, Guid toppingId)
    {
        var pizza = await _catalogDb.Pizzas
         .Include(p => p.PizzaToppings)
            .ThenInclude(pt => pt.Topping)
            .FirstOrDefaultAsync(p => p.Id == pizzaId);
        if (pizza == null)
            throw new KeyNotFoundException($"Pizza with id {pizzaId} not found");
        var topping = pizza.PizzaToppings.FirstOrDefault(pt => pt.ToppingId == toppingId);
        if (topping != null)
        {
            pizza.PizzaToppings.Remove(topping);
            await _catalogDb.SaveChangesAsync();
        }
        return pizza;
    }


    public async Task<IEnumerable<PizzaToppingDto>> GetToppingsForPizzaAsync(Guid pizzaId)
    {
        var pizza = await _catalogDb.Pizzas
          .Include(p => p.PizzaToppings)
             .ThenInclude(pt => pt.Topping)
             .FirstOrDefaultAsync(p => p.Id == pizzaId);
        if (pizza == null)
            return Enumerable.Empty<PizzaToppingDto>();

        return pizza.PizzaToppings.Select(pt => new PizzaToppingDto
        {
            Name = pt.Topping.Name,
            ExtraPrice = pt.Topping.ExtraPrice
        }).ToList();
    }
    public async Task DeleteAsync(Guid id)
    {
       var pizza =  await _catalogDb.Pizzas.FindAsync(id);
        if (pizza != null)
        {
             _catalogDb.Pizzas.Remove(pizza);
            await _catalogDb.SaveChangesAsync();
       }
        
    }
    public async Task AddAsync(Pizza pizza)
    {
        await _catalogDb.Pizzas.AddAsync(pizza);
        await _catalogDb.SaveChangesAsync();
       
    }
    public async Task UpdateAsync(Pizza pizza)
    {
        _catalogDb.Pizzas.Update(pizza);
        await _catalogDb.SaveChangesAsync();
    }

   
}