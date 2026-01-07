using CatalogService.Application.DTOs;
using CatalogService.Application.Service;
using CatalogService.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Presentation;

[ApiController]
[Route("api/[controller]")]
public class PizzaController : ControllerBase
{
    private readonly PizzaService _pizzaService;
    public PizzaController(PizzaService pizzaService)
    {
        _pizzaService = pizzaService;
    }
    [HttpPost("AddPizza")]
    public async Task<ActionResult> AddPizza(PizzaDTO pizzadto )
    {
        if (pizzadto == null) return BadRequest();

        var pizza = await _pizzaService.CreatePizzaAsync(pizzadto.Name!, pizzadto.Price, pizzadto.PizzaToppings);

        return CreatedAtAction(nameof(GetPizzaById), new { id = pizza.Id }, pizza);

    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Pizza>> GetPizzaById(Guid id)
    {
        var pizza = await _pizzaService.GetPizzaByIdAsync(id);
        if (pizza == null) return NotFound();
        return Ok(pizza);
    }
    [HttpGet]
    public async Task<ActionResult<List<Pizza>>> GetAll()
    {
        var pizzas = await _pizzaService.GetAllPizzasAsync();
        return Ok(pizzas);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Pizza pizza)
    {
        var existingPizza = await _pizzaService.GetPizzaByIdAsync(id);
        if (existingPizza == null) return NotFound();

        pizza.Id = id;
        await _pizzaService.UpdatePizzaAsync(pizza);
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _pizzaService.DeletePizzaAsync(id);
        return NoContent();
    }


    [HttpPost("{id}/topping")]
    public async Task<IActionResult> AddTopping(Guid id, [FromBody] Topping topping)
    {
        await _pizzaService.AddToppingsToPizzaAsync(id, topping.Id);
        return Ok();
    }


    [HttpDelete("{id}/topping/{toppingId}")]
     public async Task<IActionResult> RemoveTopping(Guid id, Guid toppingId)
     {
        await _pizzaService.RemoveToppingFromPizzaAsync(id, toppingId);
          return NoContent();
    }


    [HttpGet("{id}/toppings")]
    public async Task<ActionResult<List<Topping>>> GetToppings(Guid id)
    {
         var toppings = await _pizzaService.GetToppingsForPizzaAsync(id);
    if (toppings == null) return NotFound();
    return Ok(toppings);
    }
}