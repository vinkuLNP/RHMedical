using System;
using System.Collections.Generic;
using System.Text;

namespace RHMedical.Infrastructure.ViewModel
{
    public class UserVM
    {
        public Guid Id { get; set; }

        public string AzureObjectId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Status { get; set; } = "Invited";

        public DateTime InvitationSentAt { get; set; }

        public DateTime? InvitationAcceptedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
