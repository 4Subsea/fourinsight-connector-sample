using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Sample.Services.Authentication
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Get an async delegate that will provide access tokens, and potentially renew the token if required.
        /// </summary>
        Func<ValueTask<AuthenticationResult>> CreateAccessTokenFactory(string authority,
            string clientId,
            string clientSecret,
            string[] scopes);
    }
}