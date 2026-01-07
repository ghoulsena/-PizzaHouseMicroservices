using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogService.Application.DTOs;
using CatalogService.Application.Interface;
using CatalogService.Domain.Entity;

namespace CatalogService.Application.Service
{
    public class PizzaService
    {
        private readonly IPizzaRepository _pizzaRepo;

        public PizzaService(IPizzaRepository pizzaRepo)
        {
            _pizzaRepo = pizzaRepo;
        }

        public async Task<Pizza> CreatePizzaAsync(string name, int price, IEnumerable<PizzaTopping>? toppings)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название пиццы не может быть пустым");

            var pizza = Pizza.Create(name, price, toppings);

            await _pizzaRepo.AddAsync(pizza);
            return pizza;
        }

        public async Task<Pizza?> GetPizzaByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id пиццы не может быть пустым");

            return await _pizzaRepo.GetByIdAsync(id);
        }

        public async Task<List<Pizza>> GetAllPizzasAsync()
        {
            return await _pizzaRepo.GetAllAsync();
        }

        public async Task UpdatePizzaAsync(Pizza pizza)
        {
            if (string.IsNullOrWhiteSpace(pizza.Name))
                throw new ArgumentException("Название пиццы не может быть пустым");

            await _pizzaRepo.UpdateAsync(pizza);
        }

        public async Task DeletePizzaAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id пиццы не может быть пустым");

            await _pizzaRepo.DeleteAsync(id);
        }

        public async Task<Pizza?> AddOneToppingToPizzaAsync(Guid pizzaId, Guid toppingId)
        {
            if (pizzaId == Guid.Empty || toppingId == Guid.Empty)
                throw new ArgumentException("Id не может быть пустым");

            return await _pizzaRepo.AddToppingsToPizzaAsync(pizzaId, toppingId);
        }

        public async Task<Pizza?> AddToppingsToPizzaAsync(Guid pizzaId, params Guid[] toppingIds)
        {
            if (pizzaId == Guid.Empty)
                throw new ArgumentException("Id пиццы не может быть пустым");

            if (toppingIds is null || toppingIds.Length == 0)
                throw new ArgumentException("Не переданы id топпингов");

            return await _pizzaRepo.AddToppingsToPizzaAsync(pizzaId, toppingIds);
        }

        public async Task<Pizza?> RemoveToppingFromPizzaAsync(Guid pizzaId, Guid toppingId)
        {
            if (pizzaId == Guid.Empty || toppingId == Guid.Empty)
                throw new ArgumentException("Id не может быть пустым");

            return await _pizzaRepo.RemoveToppingFromPizzaAsync(pizzaId, toppingId);
        }

        public async Task<IEnumerable<PizzaToppingDto>> GetToppingsForPizzaAsync(Guid pizzaId)
        {
            if (pizzaId == Guid.Empty)
                throw new ArgumentException("Id пиццы не может быть пустым");

            return await _pizzaRepo.GetToppingsForPizzaAsync(pizzaId);
        }
    }
}
