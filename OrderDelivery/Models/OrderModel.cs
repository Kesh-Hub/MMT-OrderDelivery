using System;
using System.Collections.Generic;

namespace OrderDelivery.Models
{
    public class OrderModel
    {
        public int OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryAddress { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
        public string DeliveryExpected { get; set; }
    }
}