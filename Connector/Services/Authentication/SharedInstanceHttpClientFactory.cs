using System.Net.Http;
using Microsoft.Identity.Client;

namespace Sample.Services.Authentication
{
    public class SharedInstanceHttpClientFactory : IMsalHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        public SharedInstanceHttpClientFactory(HttpClient httpClient) => _httpClient = httpClient;

        public HttpClient GetHttpClient() => _httpClient;
    }
}