using Microsoft.EntityFrameworkCore;
using RHMedical.Domain.Entities;

namespace RHMedical.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<UserInvitation> UserInvitations => Set<UserInvitation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.AzureObjectId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(x => x.FullName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.Email).IsUnique();
                entity.HasIndex(x => x.AzureObjectId).IsUnique();
            });

            modelBuilder.Entity<UserInvitation>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.AzureObjectId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(x => x.FullName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.Email);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
