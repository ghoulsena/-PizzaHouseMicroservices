using DeliveryService.Domian.Enums;

namespace DeliveryService.Domian.Entity
{
    public class DeliveryTask
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid CourierId { get; set; }
        public DeliveryStatus Status { get; set; }

        public void AssignCourier(Guid courierId)
        {
            CourierId = courierId;
            Status = DeliveryStatus.Registered;
        }

        public static DeliveryTask Create(Guid OrderId, Guid CourierId)
        {
            return new DeliveryTask
            {
                Id = Guid.NewGuid(),
                OrderId = OrderId,
                CourierId = CourierId,
                Status = DeliveryStatus.Registered
            };
        }
    }
}
