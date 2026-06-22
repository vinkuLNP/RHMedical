using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using RHMedical.Infrastructure.ViewModel;

namespace RHMedical.Application.Identity
{
    public class AzureB2BInviteService
    {
        private readonly GraphServiceClient _graphClient;

        public AzureB2BInviteService(IConfiguration configuration)
        {
            var tenantId = configuration["AzureAd:TenantId"];
            var clientId = configuration["AzureAd:ClientId"];
            var clientSecret = configuration["AzureAd:ClientSecret"];

            var credential = new ClientSecretCredential(
                tenantId,
                clientId,
                clientSecret);

            _graphClient = new GraphServiceClient(
                credential,
                new[] { "https://graph.microsoft.com/.default" });
        }

        public async Task<AzureInviteViewModel> InviteUserAsync(
        string email,
        string fullName,
        string redirectUrl)
        {
            var invitation = new Invitation
            {
                InvitedUserEmailAddress = email,
                InvitedUserDisplayName = fullName,
                InviteRedirectUrl = redirectUrl,
                SendInvitationMessage = true,
                InvitedUserType = "Guest",
            };

            var result = await _graphClient.Invitations.PostAsync(invitation);

            if (result?.InvitedUser?.Id == null)
            {
                return new AzureInviteViewModel
                {
                    Success = false
                };
            }

            return new AzureInviteViewModel
            {
                Success = true,
                AzureObjectId = result.InvitedUser.Id,
                Email = result.InvitedUserEmailAddress ?? email,
                InviteRedeemUrl = result.InviteRedeemUrl
            };
        }
    }
}
