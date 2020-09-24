using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Sample.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SharedInstanceHttpClientFactory _httpClientFactory;

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClientFactory = new SharedInstanceHttpClientFactory(httpClient);
        }

        public Func<ValueTask<AuthenticationResult>> CreateAccessTokenFactory(
            string authority,
            string clientId,
            string clientSecret,
            string[] scopes)
        {
            var app = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithHttpClientFactory(_httpClientFactory)
                .WithClientSecret(clientSecret)
                .WithB2CAuthority(authority)
                .Build();

            async ValueTask<AuthenticationResult> AccessTokenFactory()
            {
                var response = await app
                    .AcquireTokenForClient(scopes)
                    .ExecuteAsync();
                    
                return response;
            }

            return AccessTokenFactory;
        }
    }
}