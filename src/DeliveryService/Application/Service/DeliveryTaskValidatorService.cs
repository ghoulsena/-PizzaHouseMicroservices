using DeliveryService.Application.Commot.Validators;
using DeliveryService.Application.Interface;
using DeliveryService.Domian.Entity;
using DeliveryService.Http;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Application.Service
{
    public class DeliveryTaskValidatorService
    {
        private readonly IDeliveryTaskRepository _repository;
        private readonly CourierService _courierService;
        private readonly DeliveryServiceClient _deliveryClient;

        public DeliveryTaskValidatorService(IDeliveryTaskRepository repo, CourierService courierService,
                                            DeliveryServiceClient deliveryClient)
        {
            _repository = repo;
            _courierService = courierService;
            _deliveryClient = deliveryClient;
        }
        public async Task  ValidateForCreateAsync(DeliveryTask task)
        {
            DeliveryTaskValidator.Validate(task);

            await _deliveryClient.ValidateOrderExistsAsync(task.OrderId);
            if (task.CourierId != Guid.Empty)
            {
                var courier = await _courierService.GetCourierByIdAsync(task.CourierId);
                if (courier == null)
                    throw new ValidationException($"Курьер с ID {task.CourierId} не найден");
            }

      
            if (task.Id != Guid.Empty)
            {
                var existing = await _repository.GetByIdAsync(task.Id);
                if (existing != null)
                    throw new ValidationException("Задача доставки с таким Id уже существует");
            }
        }

    }
}
