using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderDelivery.Data
{
    public interface IOrderDeliveryRepository
    {
        Task<List<OrderItem>> GetLatestOrderItemsForCustomerAsync(string customerId);
    }
}