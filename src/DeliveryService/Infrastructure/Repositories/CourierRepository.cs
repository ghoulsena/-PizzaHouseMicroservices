using DeliveryService.Application.Interface;
using DeliveryService.Domian.Entity;
using DeliveryService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Infrastructure.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        private readonly CourierDbContext _courierDbContext;
        public CourierRepository(CourierDbContext courierDbContext)
        {
            _courierDbContext = courierDbContext;
        }

        public async Task<Courier?> GetByIdAsync(Guid id) => await _courierDbContext.Couriers.FindAsync(id);

        public async Task<IEnumerable<Courier>> GetAllAsync() => await _courierDbContext.Couriers.ToListAsync();

        public async Task AddAsync(Courier courier)
        {
          

            bool CourierExists = await _courierDbContext.Couriers.AnyAsync(p => p.Id == courier.Id);
            if (CourierExists)
            {
                throw new InvalidOperationException("Курьер с таким Id уже существует.");
            }

            await _courierDbContext.Couriers.AddAsync(courier);

            await _courierDbContext.SaveChangesAsync();

        }

        public async Task UpdateAsync(Courier courier)
        {
            _courierDbContext.Update(courier);
            await _courierDbContext.SaveChangesAsync();

        }

        public async Task DeleteAsync(Guid id)
        {
            var courier = await _courierDbContext.Couriers.FindAsync(id);
            if (courier != null)
            {
                _courierDbContext.Remove(courier);
                await _courierDbContext.SaveChangesAsync();
            }
        }
    }
}
