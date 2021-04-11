using System;
using System.ComponentModel.DataAnnotations;

namespace OrderDelivery.Data
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [StringLength(10)]
        public string CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime DeliveryExpected { get; set; }

        public bool ContainsGift { get; set; }

        [StringLength(30)]
        public string ShippingMode { get; set; }

        [StringLength(30)]
        public string OrderSource { get; set; }
    }
}