using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Application.Interfaces;
using UserService.Infrastructure.UserDbContext;

namespace UserService.Infrastructure.Repository;

public class RoleRepository : IRoleRepository
{
    private readonly UserDbCont _context;
    public RoleRepository(UserDbCont context)
    {
        _context = context;
    }
    public async Task<Role?> GetByIdAsync(Guid id) => await _context.Roles.FindAsync(id);
    public async Task<Role?> GetByNameAsync(string name) => await _context.Roles.FirstOrDefaultAsync(u => u.Name == name);
    public async Task<List<Role>> GetAllAsync() => await _context.Roles.ToListAsync();
    public async Task AddAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Role role)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id)
    {
        var role =  await _context.Roles.FirstOrDefaultAsync(u => u.Id == id);
        if (role != null)
        {
             _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }   


}