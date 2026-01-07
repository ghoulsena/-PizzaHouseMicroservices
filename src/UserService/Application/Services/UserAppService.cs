using MediatR;
using System.Threading.Tasks;
using UserService.Application.DTOS;
using UserService.Domain.ValueObjects;
using UserService.Domain.Entities;
using UserService.Application.Interfaces;

namespace UserService.Application.Services
{
    public class UserAppService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMediator _mediator;
        private readonly IAuthProvider _authProvider;
        public UserAppService(IUserRepository userRepo, IAuthProvider authProvider, IMediator mediator)
        {
            _userRepo = userRepo;
            _authProvider = authProvider;
            _mediator = mediator;
        }

        public async Task<string> Login(string Email, string Password)
        {
            var user = await _userRepo.GetByEmailAsync(Email);

            if (user == null || !user.CheckPassword(Password))
            {

                throw new Exception("Пользователь не найден или пароль неверный");
            }

            return _authProvider.GenerateToken(user);
        }

        public async Task<User> RegisterAsync(string userName,string email,string password)
        {
            var emailVo = new Email(email);
            var passwordVo = new Password(password);

            if (await _userRepo.GetByEmailAsync(emailVo.Value) != null)
                throw new Exception("User already exists");

            var user = User.Create(userName, emailVo, passwordVo);

            await _userRepo.AddAsync(user);

            foreach (var domainEvent in user.DomainEvents)
                await _mediator.Publish(domainEvent);

            user.ClearDomainEvents();

            return user;
        }
        public async Task<(bool Success, string Message)> AddRoleToUserAsync(Guid userId, string roleName)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                return (false, "Пользователь не найден");

            var role = new Role(roleName);
            user.AddRole(role);

            await _userRepo.UpdateAsync(user);

            return (true, $"Пользователю {user.UserName} присвоена роль {roleName}");
        }

    }

}