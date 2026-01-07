using DeliveryService.Application.Interface;
using DeliveryService.Application.Service;
using DeliveryService.Domian.Entity;
using DeliveryService.Http;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Application.Commot.Validators
{
    public class DeliveryTaskValidatorService
    {
        private readonly IDeliveryTaskRepository _repo;
        private readonly CourierService _courierService;
        private readonly IDeliveryServiceClient _deliveryClient;

        public DeliveryTaskValidatorService(IDeliveryTaskRepository repo,
                                            CourierService courierService,
                                            DeliveryServiceClient deliveryClient)
        {
            _repo = repo;
            _courierService = courierService;
            _deliveryClient = deliveryClient;
        }

        public async Task ValidateForCreateAsync(DeliveryTask task)
        {
            DeliveryTaskValidator.Validate(task); 

            await _deliveryClient.ValidateOrderExistsAsync(task.OrderId);

            if (task.CourierId != Guid.Empty)
            {
                var courier = await _courierService.GetCourierByIdAsync(task.CourierId);
                if (courier == null)
                    throw new ValidationException($"Курьер с ID {task.CourierId} не найден.");
            }

            if (task.Id != Guid.Empty)
            {
                var existing = await _repo.GetByIdAsync(task.Id);
                if (existing != null)
                    throw new ValidationException("Задача доставки с таким Id уже существует.");
            }
        }
    }
}
