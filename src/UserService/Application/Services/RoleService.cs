using UserService.Domain.Entities;
using UserService.Application.Interfaces;

namespace UserService.Application.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _roleRepo;
        private readonly IUserRepository _userRepo;

        public RoleService(IRoleRepository roleRepo, IUserRepository userRepo)
        {
            _roleRepo = roleRepo;
            _userRepo = userRepo;
        }

        public async Task<(bool Success, string Message)> AssignRoleToUserAsync(Guid userId, string roleName)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                return (false, "Пользователь не найден");

            var role = await _roleRepo.GetByNameAsync(roleName);
            if (role == null)
            {
                role = new Role(roleName);
                await _roleRepo.AddAsync(role);
            }

            user.AddRole(role);
            await _userRepo.UpdateAsync(user);

            return (true, $"Пользователю {user.UserName} присвоена роль {roleName}");
        }
    }
}
