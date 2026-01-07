namespace DeliveryService.Application.Interface
{
    public interface IDeliveryServiceClient
    {
        Task ValidateOrderExistsAsync(Guid orderId);
    }
}
