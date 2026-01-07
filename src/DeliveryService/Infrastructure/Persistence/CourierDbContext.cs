using DeliveryService.Domian.Entity;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Infrastructure.Persistence
{
    public class CourierDbContext : DbContext
    {
        public CourierDbContext(DbContextOptions<CourierDbContext> options) : base(options) { }

        public DbSet<Courier> Couriers {get;set;}

        public DbSet<DeliveryTask> DeliveryTasks { get; set; }
    }
}
