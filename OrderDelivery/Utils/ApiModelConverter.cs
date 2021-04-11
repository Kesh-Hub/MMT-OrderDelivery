using System;
using System.Collections.Generic;
using System.Linq;
using OrderDelivery.Data;
using OrderDelivery.Models;

namespace OrderDelivery.Utils
{
    public class ApiModelConverter : IApiModelConverter
    {
        public OrderDeliveryModel ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel(List<OrderItem> orderItems, CustomerDetailDto customerDetail)
        {
            if (customerDetail == null)
                throw new ArgumentNullException(nameof(CustomerDetailDto));

            var orderDeliveryModel = new OrderDeliveryModel();

            // Build Delivery Address
            var deliveryAddressArr = new [] { $"{customerDetail.HouseNumber.ToUpper()} {customerDetail.Street}", customerDetail.Town, customerDetail.Postcode };
            var deliveryAddress = string.Join(", ", deliveryAddressArr.Where(s => !string.IsNullOrEmpty(s)));

            // Build Order Model
            var orderModel = new OrderModel();
            if (orderItems != null && orderItems.Count > 0)
            {
                orderModel.OrderNumber = orderItems.First().Order.OrderId;
                orderModel.OrderDate = orderItems.First().Order.OrderDate.ToString("dd-MMM-yyyy");
                orderModel.DeliveryAddress = deliveryAddress;
                orderModel.DeliveryExpected = orderItems.First().Order.DeliveryExpected.ToString("dd-MMM-yyyy");
                orderModel.OrderItems = orderItems.Select(orderItem => new OrderItemModel
                    {
                        Product = orderItem.Order.ContainsGift ? "Gift" : orderItem.Product.ProductName,
                        Quantity = orderItem.Quantity,
                        PriceEach = orderItem.Price
                    })
                    .ToList();

                orderDeliveryModel.Order = orderModel;
            }

            // Add CustomerModel to OrderDeliveryModel and Return
            orderDeliveryModel.Customer = new CustomerModel
            {
                FirstName = customerDetail.FirstName,
                LastName = customerDetail.LastName
            };

            return orderDeliveryModel;
        }

    }
}