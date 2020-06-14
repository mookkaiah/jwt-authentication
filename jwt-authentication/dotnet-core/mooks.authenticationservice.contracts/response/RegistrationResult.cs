namespace mooks.authenticationservice.contracts.response
{
    using System.Collections.Generic;
    using System.Net;

    public class RegistrationResult : IResult
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool Success { get; set; }

        public IEnumerable<string> Messages { get; set; }
    }
}