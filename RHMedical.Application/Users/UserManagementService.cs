using Microsoft.EntityFrameworkCore;
using RHMedical.Application.Identity;
using RHMedical.Data.Persistence;
using RHMedical.Domain.Entities;
using RHMedical.Infrastructure.ViewModel;

namespace RHMedical.Application.Users
{
    public class UserManagementService : IUserManagementService
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

        public async Task<Guid> InviteUserAsync(InviteUserViewModel request)
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

        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            return await _db.Users.AsNoTracking()
              .OrderByDescending(x => x.InvitationSentAt)
              .Select(x => new UserViewModel
              {
                 Id = x.Id,
                 Email = x.Email,
                 FullName = x.FullName,
                 Status = x.Status,
                 IsActive = x.IsActive,
                 InvitationSentAt = x.InvitationSentAt,
                 LastLoginAt = x.LastLoginAt,
              }).ToListAsync();
        }

        public async Task ToggleUserStatusAsync(Guid userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            var newStatus = !user.IsActive;

            await _inviteService.SetUserAccountEnabledAsync(user.AzureObjectId,newStatus);


            user.IsActive = newStatus;
            user.Status = user.IsActive ? "Active" : "Inactive";

            await _db.SaveChangesAsync();
        }
    }
}
