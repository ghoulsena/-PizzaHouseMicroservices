namespace OrderService.Domain.Enums;

public enum OrderStatus
{
    Pending,    // создан
    Paid,       // оплатил
    Preparing,  // приготавливается
    Delivered,  // доставляется
    Completed,  // завершён
    Cancelled,  // отменён
    Failed //ошибка
    
}