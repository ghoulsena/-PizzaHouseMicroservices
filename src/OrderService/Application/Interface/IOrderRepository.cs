using OrderService.Domain.Entities;

namespace OrderService.Application.Interface;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order?>> GetByUserIdAsync(Guid userId);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(Guid id);

}
