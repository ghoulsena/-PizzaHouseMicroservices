namespace UserService.Domain.ValueObjects;

public class Email
{
    public string Value { get; set; }
    public Email(string value)
    {
        if (!value.Contains("@")) throw new ArgumentException("Неверный email");
        Value = value;
    }
}