namespace OrderService.Application.DTOs;

public class PaymentDto
{
    public Guid OrderId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string CardHolder { get; set; } = string.Empty;
    public string Expiry { get; set; } = string.Empty;
    public string Cvc { get; set; } = string.Empty;
}
