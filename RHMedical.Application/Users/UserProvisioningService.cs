using Microsoft.EntityFrameworkCore;
using RHMedical.Data.Persistence;
using RHMedical.Domain.Entities;

namespace RHMedical.Application.Users
{
    public class UserProvisioningService : IUserProvisioningService
    {
        private readonly AppDbContext _db;

        public UserProvisioningService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User> ProvisionUserAsync(string azureObjectId, string email, string fullName)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.AzureObjectId == azureObjectId);

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    AzureObjectId = azureObjectId,
                    Email = email,
                    FullName = fullName,
                    Status = "Active",
                    InvitationAcceptedAt = DateTime.UtcNow,
                    LastLoginAt = DateTime.UtcNow,
                    IsActive = true
                };

                _db.Users.Add(user);
            }
            else
            {
                user.InvitationAcceptedAt = DateTime.UtcNow;
                user.LastLoginAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();

            return user;
        }
    }
}
