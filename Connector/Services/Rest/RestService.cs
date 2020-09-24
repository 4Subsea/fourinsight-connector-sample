using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Sample.Services.Authentication;
using Sample.Services.Configuration;

namespace Sample.Services.Rest
{
    public class RestService : IRestService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBase;
        private readonly Func<ValueTask<AuthenticationResult>> _accessToken;

        public RestService(
            IAuthenticationService authentication,
            IConfigurationService configuration,
            HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiBase = configuration.Api.Endpoint;

            _accessToken = authentication.CreateAccessTokenFactory(
                configuration.Api.Authority, 
                configuration.Api.ClientId, 
                configuration.Api.ClientSecret, 
                configuration.Api.Scopes);
        }

        public async Task<string> Get(string api)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _apiBase + api);
            request.Headers.Authorization = await CreateAuthorizationHeader();

            using var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        private async ValueTask<AuthenticationHeaderValue> CreateAuthorizationHeader()
        {
            var accessToken = await _accessToken();
            return AuthenticationHeaderValue.Parse(accessToken.CreateAuthorizationHeader());
        }
    }
}