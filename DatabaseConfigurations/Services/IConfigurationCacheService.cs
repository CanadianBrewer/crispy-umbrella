using DatabaseConfigurations.Models;

namespace DatabaseConfigurations.Services
{
    public interface IConfigurationCacheService
    {
        T CheckCache<T>(string configurationName) where T : BaseConfiguration;
    }
}