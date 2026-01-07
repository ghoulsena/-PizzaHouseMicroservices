using DeliveryService.Domian.Entity;

namespace DeliveryService.Application.Interface
{
    public interface ICourierRepository
    {
        Task<Courier?> GetByIdAsync(Guid id);

        Task<IEnumerable<Courier>> GetAllAsync();

        Task AddAsync(Courier courier);

        Task UpdateAsync(Courier courier);

        Task DeleteAsync(Guid id);
    }
}
