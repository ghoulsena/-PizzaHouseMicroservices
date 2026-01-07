namespace UserService.Domain.ValueObjects;

public class Password
{
    public string Value { get; set; }
    public Password(string value)
    {
        if (value.Length < 7) throw new ArgumentException("Пароль слишком короткий");
        Value = value;
    }
}