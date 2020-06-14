namespace mooks.authenticationservice.Data.Options
{
    using System;
    public class JwtSettings
    {
        public string Secret { get; set; }

        public TimeSpan TokenLifetime { get; set; }
    }
}
