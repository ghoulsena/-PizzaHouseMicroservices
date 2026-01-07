namespace Microsoft.CustomException;

public class OrderNotFoundException : Exception
{
    public  OrderNotFoundException(Guid orderId): base($"Заказ с  id :{orderId} не найден"){

    }
}