using UserService.Domain.ValueObjects;

namespace UserService.Application.DTOS
{
    public class UserRegisterDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}