using System.Threading.Tasks;

namespace Sample.Services.Rest
{
    public interface IRestService
    {
        Task<string> Get(string api);
        Task<TResponse> Get<TResponse>(string api);
        Task<string> Post(string api);
        Task<TResponse> Post<TResponse>(string api);
        Task<string> Post<TRequest>(string api, TRequest request);
    }
}