using System;
using System.Collections.Generic;
using System.Text;

namespace RHMedical.Infrastructure.Identity
{
    public class AzureInviteResult
    {
        public bool Success { get; set; }
        public string AzureObjectId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? InviteRedeemUrl { get; set; }
    }
}
