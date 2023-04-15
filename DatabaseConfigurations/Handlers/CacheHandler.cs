using DatabaseConfigurations.Models;
using Microsoft.Extensions.Logging;

namespace DatabaseConfigurations.Services
{
    public class CacheHandler : ICacheHandler
    {
        private readonly ILogger<CacheHandler> _logger;
        private readonly IConfigurationCacheService _cacheService;

        public CacheHandler(ILogger<CacheHandler> logger, IConfigurationCacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        public T GetConfiguration<T>(string configurationName) where T : BaseConfiguration
        {
            _logger.LogInformation($"GetConfiguration<{typeof(T)}>");
            return _cacheService.CheckCache<T>(configurationName);
        }
    }
}