using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.UserDbContext;

public class UserDbCont : DbContext
{
    public UserDbCont(DbContextOptions<UserDbCont> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }


}
