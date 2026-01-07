using DeliveryService.Domian.Entity;

namespace DeliveryService.Application.Interface
{
    public interface IDeliveryTaskRepository
    {
        Task<DeliveryTask?> GetByIdAsync(Guid id);
        Task<IEnumerable<DeliveryTask>> GetAllAsync();
        Task AddAsync(DeliveryTask task);
        Task UpdateAsync(DeliveryTask task);
        Task DeleteAsync(Guid id);
        Task<DeliveryTask?> GetByOrderIdAsync(Guid orderId);

    }
}

