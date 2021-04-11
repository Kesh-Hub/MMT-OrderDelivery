using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OrderDelivery.Data
{
    public class OrderDeliveryRepository : IOrderDeliveryRepository
    {
        private readonly OrderDeliveryContext _context;

        public OrderDeliveryRepository(OrderDeliveryContext context)
        {
            _context = context;
        }

        public async Task<List<OrderItem>> GetLatestOrderItemsForCustomerAsync(string customerId)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            var latestOrder = _context.Orders.Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate).FirstOrDefault();

            // Restriction: Return an empty order if user does not have an order
            if (latestOrder != null)
            {
                orderItems = await _context.OrderItems.Where(oi => oi.Order.OrderId == latestOrder.OrderId)
                    .Include(oi => oi.Order)
                    .Include(oi => oi.Product).ToListAsync();
            }
            
            return orderItems;
        }

    }
}