using System;
using System.Collections.Generic;
using System.Text;

namespace RHMedical.Domain.Entities
{
    public class UserInvitation
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public string AzureObjectId { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Status { get; set; } = "Invited";

        public DateTime InvitationSentAt { get; set; }

        public string? InviteRedeemUrl { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
