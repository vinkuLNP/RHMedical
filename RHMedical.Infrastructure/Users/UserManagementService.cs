using Microsoft.EntityFrameworkCore;
using RHMedical.Application.Users;
using RHMedical.Domain.Entities;
using RHMedical.Infrastructure.Identity;
using RHMedical.Infrastructure.Persistence;

namespace RHMedical.Infrastructure.Users
{
    public class UserManagementService
    {
        private readonly AppDbContext _db;
        private readonly AzureB2BInviteService _inviteService;

        public UserManagementService(
        AppDbContext db,
        AzureB2BInviteService inviteService)
        {
            _db = db;
            _inviteService = inviteService;
        }

        public async Task<Guid> InviteUserAsync(InviteUserRequest request)
        {
            var email = request.Email.Trim().ToLower();

            var existingUser = await _db.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            if (existingUser != null)
            {
                throw new InvalidOperationException("User already exists.");
            }

            var inviteResult = await _inviteService.InviteUserAsync(
                email,
                request.FullName,
                request.RedirectUrl);

            if (!inviteResult.Success)
            {
                throw new InvalidOperationException("Azure invitation failed.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                AzureObjectId = inviteResult.AzureObjectId,
                Email = email,
                FullName = request.FullName,
                Status = "Invited",
                InvitationSentAt = DateTime.UtcNow,
                IsActive = true
            };

            var invitation = new UserInvitation
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                AzureObjectId = inviteResult.AzureObjectId,
                Email = email,
                FullName = request.FullName,
                Status = "Invited",
                InvitationSentAt = DateTime.UtcNow,
                InviteRedeemUrl = inviteResult.InviteRedeemUrl
            };

            _db.Users.Add(user);
            _db.UserInvitations.Add(invitation);

            await _db.SaveChangesAsync();

            return user.Id;
        }
    }
}
