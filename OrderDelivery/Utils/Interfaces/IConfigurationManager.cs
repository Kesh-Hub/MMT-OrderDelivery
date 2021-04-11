namespace OrderDelivery.Utils
{
    public interface IConfigurationManager
    {
        string CustomerDetailApiBaseUrl { get; }
        string CustomerDetailApiKeyName { get; }
        string CustomerDetailApiKeyValue { get; }
    }
}