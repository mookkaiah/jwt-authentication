namespace mooks.authenticationservice.contracts.requests
{
    using System.ComponentModel.DataAnnotations;
    public class ChangePassword
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}