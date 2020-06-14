namespace mooks.authenticationservice.Domain.Identity
{
    using System;
    public class User
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}