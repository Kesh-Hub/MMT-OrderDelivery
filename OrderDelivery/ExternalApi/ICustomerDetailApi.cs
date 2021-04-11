using System.Threading.Tasks;
using OrderDelivery.Data;

namespace OrderDelivery.ExternalApi
{
    public interface ICustomerDetailApi
    {
        Task<CustomerDetailDto> GetUserDetailsAsync(string emailAddress);
    }
}