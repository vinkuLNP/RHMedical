using RHMedical.Domain.Entities;

namespace RHMedical.Application.Users
{
    public interface IUserProvisioningService
    {
        Task<User> ProvisionUserAsync(string azureObjectId, string email, string fullName);
    }
}
