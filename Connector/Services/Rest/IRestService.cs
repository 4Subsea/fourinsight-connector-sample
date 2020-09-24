using System.Threading.Tasks;

namespace Sample.Services.Rest
{
    public interface IRestService
    {
        Task<string> Get(string api);
    }
}