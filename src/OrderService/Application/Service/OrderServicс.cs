using OrderService.Application.Interface;

using OrderService.Domain.Entities;
using OrderService.Application.DTOs;
using OrderService.Domain.Enums;
using Microsoft.CustomException;


namespace OrderService.Application.Service;

public class OrderServicee
{
    private readonly IOrderRepository _orderRepos;

    private readonly ICatalogHttpClient _catalogClient;

    public OrderServicee(IOrderRepository orderRepos,  ICatalogHttpClient catalogClient)
    {
        _orderRepos = orderRepos;
        _catalogClient = catalogClient;
    }

    public async Task<Order?> GetOrderByIdAsync(Guid id)
    {
        return await _orderRepos.GetByIdAsync(id);
    }



    public async Task<IEnumerable<Order?>> GetOrdersByUserIdAsync(Guid userId)
    {
    var orders = await _orderRepos.GetByUserIdAsync(userId);
    
    if (orders == null || !orders.Any())
            throw new OrderNotFoundException(userId);
    return orders;
    }

    public async Task UpdateOrderAsync(Order order)
    {
        await _orderRepos.UpdateAsync(order);
    }


    public async Task DeleteOrderAsync(Guid id)
    {
        await _orderRepos.DeleteAsync(id);
    }

    public async Task<bool> PayOrderAsync(Guid orderId, Payment payment)
    {
        var order = await GetOrderByIdAsync(orderId);
        if (order == null) return false;

        if (payment.CardNumber == "0000 0000 0000 0000")
        {
            order.Status = OrderStatus.Paid;
        }
        else
        {
            order.Status = OrderStatus.Failed;
        }

        await UpdateOrderAsync(order);
        return order.Status == OrderStatus.Paid;
        
    }
    public async Task<Order> CreateOrderAsync(Guid userId, List<OrderItem> items)
    {

        foreach (var item in items)
        {
            var pizza = await _catalogClient.GetPizzaAsync(item.PzzaId);
            if (pizza == null)
                throw new Exception($"Pizza {item.PzzaId} не найдена");

            item.Price = pizza.Price; 
        }


        var order = Order.CreateNew(userId, items);

        await _orderRepos.AddAsync(order);

        return order;
    }

}
