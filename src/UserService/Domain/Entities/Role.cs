namespace UserService.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Role(string name)
    {
        Name = name ?? string.Empty;
    }
}