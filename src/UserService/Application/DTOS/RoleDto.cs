namespace UserService.Application.DTOS;

public class RoleDto
{
    public Guid UserId { get; set; }
    public string RoleName { get; set; } = string.Empty;

}