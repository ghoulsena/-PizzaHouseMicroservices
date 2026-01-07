namespace UserService.Application.Interfaces;
using UserService.Domain.Entities;

public interface IAuthProvider
{
    string GenerateToken(User user);
}
