using Microsoft.EntityFrameworkCore;
using RHMedical.Domain.Entities;
using System.Reflection;

namespace RHMedical.Data.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<UserInvitation> UserInvitations => Set<UserInvitation>();

        protected override void OnModelCreating(ModelBuilder builder)

        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
