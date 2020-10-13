using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
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
            var request = await CreateRequest(HttpMethod.Get, api);
            return await Send(request);
        }

        public async Task<TResponse> Get<TResponse>(string api)
        {
            var content = await Get(api);
            return JsonSerializer.Deserialize<TResponse>(content);
        }

        public async Task<string> Post(string api)
        {
            var request = await CreateRequest(HttpMethod.Post, api);
            return await Send(request);
        }

        public async Task<TResponse> Post<TResponse>(string api)
        {
            var request = await CreateRequest(HttpMethod.Post, api);
            var content = await Send(request);
            return JsonSerializer.Deserialize<TResponse>(content);
        }

        public async Task<string> Post<TRequest>(string api, TRequest content)
        {
            var request = await CreateRequest(HttpMethod.Post, api);
            request.Content = CreateStringContent(content);
            return await Send(request);
        }

        private async Task<string> Send(HttpRequestMessage request)
        {
            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private static StringContent CreateStringContent<TRequest>(TRequest content)
        {
            return new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8,
                MediaTypeNames.Application.Json);
        }

        private async Task<HttpRequestMessage> CreateRequest(HttpMethod method, string api)
        {
            var request = new HttpRequestMessage(method, _apiBase + api);
            request.Headers.Authorization = await CreateAuthorizationHeader();
            return request;
        }

        private async ValueTask<AuthenticationHeaderValue> CreateAuthorizationHeader()
        {
            var accessToken = await _accessToken();
            return AuthenticationHeaderValue.Parse(accessToken.CreateAuthorizationHeader());
        }
    }
}