namespace RHMedical.Application.Users
{
    public class InviteUserRequest
    {

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string RedirectUrl { get; set; } = string.Empty;
    }
}
