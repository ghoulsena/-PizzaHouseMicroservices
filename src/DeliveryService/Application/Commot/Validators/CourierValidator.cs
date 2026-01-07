using DeliveryService.Domian.Entity;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Application.Commot.Validators
{
    public static  class CourierValidator
    {
        public static void Validate(Courier courier)
        {
            if (string.IsNullOrEmpty(courier.Name))
            {
                throw new ValidationException("Имя курьера не может быть пустым");
            }
            if (courier.Id == Guid.Empty){
                throw new ValidationException("Id курьера не может быть пустым");
            }
            if(courier.Name.Length < 2)
            {
                throw new ValidationException("Имя курьер должно быть не менее 2 символов");
            }

        }
    }
}
