using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Domain.Entities;
using OrderService.Application.DTOs;
using OrderService.Application.Service;
using OrderService.Mapper;
using OrderService.Presentation.Helpers;

using System.Security.Claims;

namespace OrderService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderServicee _orderService;
    private readonly IAuthorizationService _authorizationService;
    private readonly ILogger _logger;

    public OrderController(OrderServicee orderService, IAuthorizationService authorizationService,ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _authorizationService = authorizationService;
        _logger = logger;

    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetOrderById([FromRoute] Guid id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null) return NotFound();
        var dto = OrderMapper.ToDto(order);
        

        return Ok(dto);
    }

    [HttpGet("user/{userId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserId([FromRoute] Guid userId)
    {
        var orders = await _orderService.GetOrdersByUserIdAsync(userId);

        var dtos = orders
            .Where(o => o != null)
            .Select(o => new OrderDto
            {
                OrderId = o!.Id,
                UserId = o.UserId,
                TotalPrice = o.TotalPrice,
                Status = o.Status,
                CreatedAt = o.CreatedAt
            });

        return Ok(dtos);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] OrderDto orderDto)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized("Пользователь не найден");

        var userId = Guid.Parse(userIdClaim.Value);


        var items = orderDto.Items.Select(i => new OrderItem
        {
            PzzaId = i.PzzaId,
            Quantity = i.Quantity
        }).ToList();


        var createdOrder = await _orderService.CreateOrderAsync(userId, items);

        var resultDto = OrderMapper.ToDto(createdOrder);
        

        return CreatedAtAction(nameof(GetOrderById), new { id = resultDto.OrderId }, resultDto);
    }




    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrder(Guid id, [FromBody] OrderDto dto)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if(order == null)
        {
            return BadRequest("Заказ с таким Id не существует");
        }
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!await _authorizationService.AuthorizeResourceAsync(User, order, "OrderOwner"))
        {
            _logger.LogWarning("Authorization failed. Token userId: {TokenUserId}, Order userId: {OrderUserId}",
                               userIdClaim, order.UserId);
            return Forbid();
        }

        if (id != dto.OrderId) return BadRequest("Id mismatch");

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return BadRequest("Некорректный UserId");
        }

        var neworder = OrderMapper.ToDomain(dto, userId);


        await _orderService.UpdateOrderAsync(neworder);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder([FromRoute] Guid id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound();

        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;


        _logger.LogInformation("DeleteOrder called. Token userId: {TokenUserId}, Order userId: {OrderUserId}, Order Id: {OrderId}",
                               userIdClaim, order.UserId, order.Id);


        if (!await _authorizationService.AuthorizeResourceAsync(User, order, "OrderOwner"))
        {
            _logger.LogWarning("Authorization failed. Token userId: {TokenUserId}, Order userId: {OrderUserId}",
                               userIdClaim, order.UserId);
            return Forbid();
        }

        await _orderService.DeleteOrderAsync(id);
        _logger.LogInformation("Order deleted. Id: {OrderId}", order.Id);
        return NoContent();
    }


    [HttpPost("pay")]
    public async Task<ActionResult> Pay([FromBody] PaymentDto dto)
    {
        var payment = new Payment
        {
            OrderId = Guid.NewGuid(),
            CardNumber = "1111 2222 3333 4444",
            CardHolder = "John Doe",
            Expiry = "01/30",
            Cvc = "999"
        };

        var success = await _orderService.PayOrderAsync(dto.OrderId, payment);
        if (!success) return BadRequest(new { Status = "Failed", OrderId = dto.OrderId });

        return Ok(new { Status = "Paid", OrderId = dto.OrderId });
    }

    [AllowAnonymous]
    [HttpPost("temp")]
    public async Task<ActionResult<OrderDto>> CreateTempOrder([FromBody] OrderDto dto)
    {
        var tempUserId = Guid.NewGuid();

        var items = dto.Items.Select(i => new OrderItem
        {
            PzzaId = i.PzzaId,
            Quantity = i.Quantity
        }).ToList();

        var createdOrder = await _orderService.CreateOrderAsync(tempUserId, items);
        var resultDto = OrderMapper.ToDto(createdOrder);
        

        return CreatedAtAction(nameof(GetOrderById), new { id = resultDto.OrderId }, resultDto);
    }

}
