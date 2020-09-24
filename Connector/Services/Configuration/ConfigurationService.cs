using System;
using Microsoft.Extensions.Configuration;
using Sample.Services.Configuration.Models;

namespace Sample.Services.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRoot _configuration;

        public ConfigurationService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: false);

            _configuration = builder.Build();
        }

        public Models.Api Api => _configuration.GetSection("Api").Get<Models.Api>();
    }
}