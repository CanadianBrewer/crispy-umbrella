using DatabaseConfigurations.Models;
using LazyCache;
using LazyCache.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace DatabaseConfigurations.Services
{
    public class ConfigurationCacheService : IConfigurationCacheService
    {
        private readonly ILogger<ConfigurationCacheService> _logger;
        private IAppCache _cache = new CachingService();

        public ConfigurationCacheService(ILogger<ConfigurationCacheService> logger)
        {
            _logger = logger;
            InitializeOrResetCache();
        }

        public T CheckCache<T>(string configurationName) where T : BaseConfiguration
        {
            _logger.LogInformation($"CheckCache<{typeof(T)}>({configurationName})");
            Func<T> config = () => LoadConfiguration<T>(configurationName);
            T cachedResult = _cache.GetOrAdd(configurationName, config, DateTimeOffset.UtcNow.AddMinutes(15));
            return cachedResult;
        }

        private T LoadConfiguration<T>(string configurationName) where T : BaseConfiguration
        {
            // get the specific config item from the db here; use a local file for now since cache should not reference IConfigurationRoot
            IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("config.json").Build();
            T loadedConfig = configuration.GetSection(configurationName).Get<T>();
            return loadedConfig ?? null;
        }

        private void InitializeOrResetCache()
        {
            // this should be async since we are making a call out, but calling async methods from a ctor is messy
            // and we really need this to fully complete before moving on.
            _logger.LogInformation("InitResetCache");
            _cache.CacheProvider.Dispose();
            var provider = new MemoryCacheProvider(new MemoryCache(new MemoryCacheOptions()));
            _cache = new CachingService(provider);

            // populate the cache from the db here using an API call
            // use a local file for now since cache should not reference IConfigurationRoot
            IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("config.json").Build();
            FooConfiguration fc = configuration.GetSection("FooConfiguration").Get<FooConfiguration>();
            _cache.Add(fc.Name, fc, DateTimeOffset.UtcNow.AddMinutes(15));
            BarConfiguration bc = configuration.GetSection("BarConfiguration").Get<BarConfiguration>();
            _cache.Add(bc.Name, bc, DateTimeOffset.UtcNow.AddMinutes(15));
        }
    }
}