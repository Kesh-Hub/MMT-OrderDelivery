using System.Collections.Generic;
using OrderDelivery.Data;
using OrderDelivery.Models;

namespace OrderDelivery.Utils
{
    public interface IApiModelConverter
    {
        OrderDeliveryModel ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel(List<OrderItem> orderItems,
            CustomerDetailDto customerDetail);
    }
}