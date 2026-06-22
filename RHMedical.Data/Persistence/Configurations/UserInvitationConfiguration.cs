using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RHMedical.Domain.Entities;

namespace RHMedical.Data.Persistence.Configurations
{
    public class UserInvitationConfiguration : IEntityTypeConfiguration<UserInvitation>
    {
        public void Configure(EntityTypeBuilder<UserInvitation> builder)

        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AzureObjectId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.Email);
        }
    }
}
