using DeliveryService.Application.Commot.Validators;
using DeliveryService.Application.Interface;
using DeliveryService.Domian.Entity;
using DeliveryService.Http;
using DeliveryService.Infrastructure.Repositories;
using System.Diagnostics.Metrics;

namespace DeliveryService.Application.Service
{
    public class DeliveryTaskService
    {
        private readonly IDeliveryTaskRepository _deliveryTaskRepository;
        DeliveryTaskValidatorService _DeliveryTaskValidatorService;

        public DeliveryTaskService(IDeliveryTaskRepository repository, DeliveryTaskValidatorService deliverytaskservice)
        {
            _deliveryTaskRepository = repository;
            _DeliveryTaskValidatorService = deliverytaskservice;
        }   

        public async Task<IEnumerable<DeliveryTask>> GetAllTasksAsync()
        {
            return await _deliveryTaskRepository.GetAllAsync();
        }

        public async Task<DeliveryTask?> GetTaskByIdAsync(Guid id)
        {
            return await _deliveryTaskRepository.GetByIdAsync(id);
        }

        public async Task<DeliveryTask?> GetTaskByOrderIdAsync(Guid orderId)
        {
            return await _deliveryTaskRepository.GetByOrderIdAsync(orderId);
        }

       
        public async Task<DeliveryTask> CreateDeliveryTaskAsync(Guid OrderId, Guid CourierId)
        {
            var task = DeliveryTask.Create(OrderId,CourierId);

            await _DeliveryTaskValidatorService.ValidateForCreateAsync(task);

            await _deliveryTaskRepository.AddAsync(task);

            return task;
        }

        public async Task UpdateTaskAsync(DeliveryTask task)
        {
            await _deliveryTaskRepository.UpdateAsync(task);
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            await _deliveryTaskRepository.DeleteAsync(id);
        }
    }
}
