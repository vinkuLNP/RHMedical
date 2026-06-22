namespace RHMedical.Infrastructure.ViewModel
{
    public class AzureInviteViewModel
    {
        public bool Success { get; set; }
        public string AzureObjectId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? InviteRedeemUrl { get; set; }
    }
}
