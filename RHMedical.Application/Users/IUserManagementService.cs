using RHMedical.Infrastructure.ViewModel;

namespace RHMedical.Application.Users
{
    public interface IUserManagementService
    {
        Task<Guid> InviteUserAsync(InviteUserViewModel request);

        Task<List<UserViewModel>> GetUsersAsync();

        Task ToggleUserStatusAsync(Guid userId);
    }
}
