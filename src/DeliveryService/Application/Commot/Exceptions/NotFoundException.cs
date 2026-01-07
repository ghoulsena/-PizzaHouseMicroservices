namespace DeliveryService.Application.Commot.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity, object key) :
            base($"{entity} с ключом {key} не найдена")
        { }
        public NotFoundException(string entity) :
            base($"Объект{entity} не найден")
        {
        }
    }
}
