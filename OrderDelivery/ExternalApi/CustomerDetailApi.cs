using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OrderDelivery.Data;
using OrderDelivery.Utils;

namespace OrderDelivery.ExternalApi
{
    public class CustomerDetailApi : ICustomerDetailApi
    {
        private readonly IConfigurationManager _configurationManager;

        public CustomerDetailApi(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(IConfigurationManager));
        }

        public async Task<CustomerDetailDto> GetUserDetailsAsync(string emailAddress)
        {
            CustomerDetailDto customerDetail = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var baseUrl = _configurationManager.CustomerDetailApiBaseUrl;
                    var uriBuilder = new UriBuilder($"{baseUrl}/GetUserDetails")
                    {
                        Query = $"email={emailAddress}"
                    };

                    // Add API Key to header
                    client.DefaultRequestHeaders.Add(_configurationManager.CustomerDetailApiKeyName,
                        _configurationManager.CustomerDetailApiKeyValue);

                    var response = await client.GetAsync(uriBuilder.Uri);
                    customerDetail = await response.Content.ReadAsAsync<CustomerDetailDto>();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError($"Failed to call GetUserDetails. Exception: {e}");
            }

            return customerDetail;
        }
    }
}