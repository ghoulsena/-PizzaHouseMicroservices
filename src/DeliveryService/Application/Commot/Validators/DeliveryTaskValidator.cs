using DeliveryService.Domian.Entity;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Application.Commot.Validators
{
    public static class DeliveryTaskValidator
    {
        public static void Validate(DeliveryTask task)
        {
            if (task.OrderId == Guid.Empty)
                throw new ValidationException("OrderId не может быть пустым.");
            if (task.Id == Guid.Empty)
         
                throw new ValidationException("Id курьера не может быть пустым");
 
        }
    }
}
