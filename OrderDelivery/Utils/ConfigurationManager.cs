using Microsoft.Azure;

namespace OrderDelivery.Utils
{
    public class ConfigurationManager : IConfigurationManager
    {
        public string CustomerDetailApiBaseUrl => GetSetting(ConfigApplicationSettings.CustomerDetailApiBaseUrl);
        public string CustomerDetailApiKeyName => GetSetting(ConfigApplicationSettings.CustomerDetailApiKeyName);
        public string CustomerDetailApiKeyValue => GetSetting(ConfigApplicationSettings.CustomerDetailApiKeyValue);


        private string GetSetting(string name)
        {
            return CloudConfigurationManager.GetSetting(name, false);
        }
    }
}