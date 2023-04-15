using DatabaseConfigurations.Models;

namespace DatabaseConfigurations.Services
{
    public interface ICacheHandler
    {
        T GetConfiguration<T>(string configurationName) where T : BaseConfiguration;
    }
}