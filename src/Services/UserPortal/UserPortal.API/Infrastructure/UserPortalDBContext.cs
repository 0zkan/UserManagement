using Microsoft.EntityFrameworkCore;
using UserManagement.Services.UserPortal.API.Entities;

namespace UserManagement.Services.UserPortal.API.Infrastructure;

public class UserPortalDBContext : DbContext
{
    public UserPortalDBContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}