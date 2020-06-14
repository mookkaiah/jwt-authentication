
namespace mooks.authenticationservice.contracts.requests
{
    using System.ComponentModel.DataAnnotations;
    public class RegisterUser
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
