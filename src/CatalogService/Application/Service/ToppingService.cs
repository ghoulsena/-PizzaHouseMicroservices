using CatalogService.Application.Interface;
using CatalogService.Domain.Entity;


namespace CatalogService.Application.Service
{
    public class ToppingService
    {
        private readonly IToppingRepository _toppingRepository;

        public ToppingService(IToppingRepository toppingRepository)
        {
            _toppingRepository = toppingRepository;
        }

        public async Task<List<Topping>> GetAllToppingsAsync()
        {
            return await _toppingRepository.GetAllAsync();
        }


        public async Task<Topping?> GetToppingByIdAsync(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new ArgumentException("Id ингридиента не может быть пустым");
            }
            return await _toppingRepository.GetByIdAsync(id);
        }


        public async Task<Topping> AddToppingAsync(string name, int extraPrice, List<Guid> pizzaIds)
        {
            var pizzatoppings = pizzaIds.Select(id =>
                new PizzaTopping { PizzaId = id }
            ).ToList();

            var topping = Topping.Create(name, extraPrice, pizzatoppings);

            await _toppingRepository.AddAsync(topping);

            return topping;
        }


        public async Task UpdateToppingAsync(Topping topping)
        {
            await _toppingRepository.UpdateAsync(topping);
        }

     
        public async Task DeleteToppingAsync(Guid id)
        {
            await _toppingRepository.DeleteAsync(id);
        }


        public async Task<List<Topping>> GetToppingsForPizzaAsync(Guid pizzaId)
        {
            var pizza = await _toppingRepository.GetPizzaWithToppingsAsync(pizzaId);
            if (pizza == null)
                return new List<Topping>();

            return pizza.PizzaToppings.Select(pt => pt.Topping).ToList();
        }

    }
}
