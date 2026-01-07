using CatalogService.Application.DTOs;
using CatalogService.Application.Service;
using CatalogService.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToppingController : ControllerBase
    {
        private readonly ToppingService _toppingService;

        public ToppingController(ToppingService toppingService)
        {
            _toppingService = toppingService;
        }

   
        [HttpGet]
        public async Task<ActionResult<List<Topping>>> GetAll()
        {
            var toppings = await _toppingService.GetAllToppingsAsync();
            return Ok(toppings);
        }

     
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Topping>> GetById(Guid id)
        {
            var topping = await _toppingService.GetToppingByIdAsync(id);
            if (topping == null) return NotFound();
            return Ok(topping);
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] PizzaToppingDto dto)
        {
            if (dto == null) return BadRequest();

            var topping = await _toppingService.AddToppingAsync(
                dto.Name!,
                dto.ExtraPrice,
                dto.PizzaIds
            );

            return CreatedAtAction(nameof(GetById), new { id = topping.Id }, topping);
        }




        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Topping topping)
        {
            if (id != topping.Id) return BadRequest("Id mismatch");
            await _toppingService.UpdateToppingAsync(topping);
            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _toppingService.DeleteToppingAsync(id);
            return NoContent();
        }

        [HttpGet("pizza/{pizzaId:guid}")]
        public async Task<ActionResult<List<Topping>>> GetToppingsForPizza(Guid pizzaId)
        {
            var toppings = await _toppingService.GetToppingsForPizzaAsync(pizzaId);
            return Ok(toppings);
        }
    }
}
