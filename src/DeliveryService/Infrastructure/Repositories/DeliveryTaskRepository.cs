using DeliveryService.Application.Interface;
using DeliveryService.Domian.Entity;
using DeliveryService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;


namespace DeliveryService.Infrastructure.Repositories
{
    public class DeliveryTaskRepository : IDeliveryTaskRepository
    {
        private readonly CourierDbContext _courierDbContext;
        public DeliveryTaskRepository(CourierDbContext courierDbContext)
        {
            _courierDbContext = courierDbContext;
        }
        public async Task<DeliveryTask?> GetByIdAsync(Guid id) => await _courierDbContext.DeliveryTasks.FindAsync(id);
        public async Task<IEnumerable<DeliveryTask>> GetAllAsync() => await _courierDbContext.DeliveryTasks.ToListAsync();
        public async Task AddAsync(DeliveryTask task)
        {
            if (task.Id == Guid.Empty)
                task.Id = Guid.NewGuid();

            await _courierDbContext.DeliveryTasks.AddAsync(task);

            await _courierDbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(DeliveryTask task)
        {
           _courierDbContext.Update(task);
            await _courierDbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var deliveryTask = await _courierDbContext.DeliveryTasks.FindAsync(id);
            if (deliveryTask != null)
            {
                _courierDbContext.Remove(deliveryTask);
                await _courierDbContext.SaveChangesAsync();
            }
        }
        public async Task<DeliveryTask?> GetByOrderIdAsync(Guid orderId) => await _courierDbContext.DeliveryTasks.FirstOrDefaultAsync(p => p.OrderId == orderId);
    }
}
