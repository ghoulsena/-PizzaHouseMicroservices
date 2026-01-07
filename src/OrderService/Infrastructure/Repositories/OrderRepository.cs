using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interface;
using OrderService.Infrastructure.Persistence;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _OrderDbCont;


    public OrderRepository(OrderDbContext OrderDbCont)
    {
        _OrderDbCont = OrderDbCont;
    }
    
    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _OrderDbCont.Order
            .AsNoTracking() 
            .FirstOrDefaultAsync(o => o.Id == id) ??  throw new Exception("Order not found");
    }


    public async Task<IEnumerable<Order?>> GetByUserIdAsync(Guid userId) => await _OrderDbCont.Order.Where(o => o.UserId == userId).ToListAsync();
    public async Task AddAsync(Order order)
    {
        _OrderDbCont.Add(order);
        await _OrderDbCont.SaveChangesAsync();
    }
    public async Task UpdateAsync(Order order)
    {
        _OrderDbCont.Update(order);
        await _OrderDbCont.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id)
    {
        var order = await _OrderDbCont.Order.FindAsync(id);
        if (order != null)
        {
            _OrderDbCont.Remove(order);
            await _OrderDbCont.SaveChangesAsync();
        }
    }
    

}