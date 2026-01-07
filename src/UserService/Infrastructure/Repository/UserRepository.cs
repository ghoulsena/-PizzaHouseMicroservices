using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;
using UserService.Infrastructure.UserDbContext;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserDbCont _context;
    public UserRepository(UserDbCont context)
    {
        _context = context;
    }
    public async Task<User?> GetByIdAsync(Guid id) => await _context.Users.FindAsync(id) ??  throw new Exception("User not found");

    public async Task<User?> GetByEmailAsync(string email) => await _context.Users.FirstOrDefaultAsync(u => u.EmailValue == email);

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }


}

