using Sample.Services.Configuration.Models;

namespace Sample.Services.Configuration
{
    public interface IConfigurationService
    {
        Models.Api Api { get; }
    }
}