using DeliveryService.Application.Commot.Exceptions;
using DeliveryService.Application.Commot.Validators;
using DeliveryService.Application.Interface;
using DeliveryService.Domian.Entity;

namespace DeliveryService.Application.Service
{
    public class CourierService
    {
        private readonly ICourierRepository _courierRepository;

        public CourierService(ICourierRepository courierRepository)
        {
            _courierRepository = courierRepository;
        }

        public async Task<IEnumerable<Courier>> GetAllCouriersAsync()
        {
            return await _courierRepository.GetAllAsync(); 
        }

        public async Task<Courier?> GetCourierByIdAsync(Guid id)
        {
            var courier = await _courierRepository.GetByIdAsync(id);
            if (courier == null)
                throw new NotFoundException("Курьер ", id);
            CourierValidator.Validate(courier);
            return courier;
        }
        public async Task AddCourierAsync(string name)
        {
            var courier = Courier.Create(name);


            CourierValidator.Validate(courier);
            

            await _courierRepository.AddAsync(courier);

        }

        public async Task UpdateCourierAsync(Courier courier)
        {
            if(courier is  null)
            {
                throw new NotFoundException("Курьер");
            }
            await _courierRepository.UpdateAsync(courier);
        }

        public async Task DeleteCourierAsync(Guid id)
        {
            await _courierRepository.DeleteAsync(id);
        }
    }
}
