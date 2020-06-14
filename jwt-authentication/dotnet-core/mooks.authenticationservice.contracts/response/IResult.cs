namespace mooks.authenticationservice.contracts.response
{
    using System.Collections.Generic;
    using System.Net;
    public interface IResult
    {
        HttpStatusCode StatusCode { get; set; }

        bool Success { get; set; }

        IEnumerable<string> Messages { get; set; }
    }
}
