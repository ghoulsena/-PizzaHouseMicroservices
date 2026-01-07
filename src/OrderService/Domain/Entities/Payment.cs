namespace OrderService.Domain.Entities;

public class Payment
{
    public Guid OrderId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string CardHolder { get; set; } = string.Empty;
    public string Expiry { get; set; } = string.Empty;
    public string Cvc { get; set; } = string.Empty;
}
