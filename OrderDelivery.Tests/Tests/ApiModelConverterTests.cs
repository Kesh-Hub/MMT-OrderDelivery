using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderDelivery.Utils;
using System.Collections.Generic;
using System.Linq;
using OrderDelivery.Data;

namespace OrderDelivery.Tests
{
    [TestClass]
    public class ApiModelConverterTests
    {
        [TestMethod]
        public void ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel_InvalidCustomerDetailDto()
        {
            // Arrange
            var apiModelConverter = new ApiModelConverter();
            var orderItems = new List<OrderItem>();
            CustomerDetailDto customerDetail = null;

            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => apiModelConverter.ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel(orderItems, customerDetail));
        }

        [TestMethod]
        public void ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel_EmptyOrderList_ReturnsValidModel()
        {
            // Arrange
            var apiModelConverter = new ApiModelConverter();
            var orderItems = new List<OrderItem>();
            var customerDetail = new CustomerDetailDto
            {
                CustomerId = "ABC123",
                Email = "Test@email.com",
                FirstName = "Mr",
                LastName = "Blue",
                LastLoggedIn = DateTime.UtcNow.ToString(),
                HouseNumber = "1",
                Street = "Street",
                Town = "Big Town",
                Postcode = "PO5T C0D3"
            };

            // Act
            var result = apiModelConverter.ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel(orderItems, customerDetail);

            // Assert
            Assert.AreEqual("Mr", result.Customer.FirstName);
            Assert.AreEqual("Blue", result.Customer.LastName);
            Assert.IsNull(result.Order);
        }

        [TestMethod]
        public void ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel_DatesAreFormattedCorrectly()
        {
            // Arrange
            var apiModelConverter = new ApiModelConverter();
            var orderItems = new List<OrderItem>()
            {
                new OrderItem()
                {
                    Order = new Order()
                    {
                        OrderId = 1,
                        DeliveryExpected = DateTime.Today,
                        OrderDate = DateTime.Today
                    },
                    Price = 1,
                    Quantity = 1,
                    Product = new Product()
                    {
                        ProductName = "Best product"
                    }
                }
            };

            var customerDetail = new CustomerDetailDto
            {
                CustomerId = "ABC123",
                Email = "Test@email.com",
                FirstName = "Mr",
                LastName = "Blue",
                LastLoggedIn = DateTime.UtcNow.ToString(),
                HouseNumber = "1",
                Street = "Street",
                Town = "Big Town",
                Postcode = "PO5T C0D3"
            };

            // Act
            var result = apiModelConverter.ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel(orderItems, customerDetail);

            // Assert
            Assert.AreEqual("Mr", result.Customer.FirstName);
            Assert.AreEqual("Blue", result.Customer.LastName);
            Assert.AreEqual(DateTime.Today.ToString("dd-MMM-yyyy"), result.Order.DeliveryExpected);
            Assert.AreEqual(DateTime.Today.ToString("dd-MMM-yyyy"), result.Order.OrderDate);
        }

        [TestMethod]
        public void ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel_OrderIsAGift_ReturnsProductNameAsGift()
        {
            // Arrange
            var apiModelConverter = new ApiModelConverter();
            var orderItems = new List<OrderItem>()
            {
                new OrderItem()
                {
                    Order = new Order()
                    {
                        OrderId = 1,
                        DeliveryExpected = DateTime.Today,
                        OrderDate = DateTime.Today,
                        ContainsGift = true
                    },
                    Price = 1,
                    Quantity = 1,
                    Product = new Product()
                    {
                        ProductName = "Best product"
                    }
                }
            };

            var customerDetail = new CustomerDetailDto
            {
                CustomerId = "ABC123",
                Email = "Test@email.com",
                FirstName = "Mr",
                LastName = "Blue",
                LastLoggedIn = DateTime.UtcNow.ToString(),
                HouseNumber = "1",
                Street = "Street",
                Town = "Big Town",
                Postcode = "PO5T C0D3"
            };

            // Act
            var result = apiModelConverter.ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel(orderItems, customerDetail);

            // Assert
            Assert.AreEqual("Mr", result.Customer.FirstName);
            Assert.AreEqual("Blue", result.Customer.LastName);
            Assert.IsTrue(result.Order.OrderItems.All(x => x.Product == "Gift"));
        }


        [TestMethod]
        public void ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel_DeliveryAddressIsFormattedCorrectly()
        {
            // Arrange
            var apiModelConverter = new ApiModelConverter();
            var orderItems = new List<OrderItem>()
            {
                new OrderItem()
                {
                    Order = new Order()
                    {
                        OrderId = 1,
                        DeliveryExpected = DateTime.Today,
                        OrderDate = DateTime.Today,
                    },
                    Price = 1,
                    Quantity = 1,
                    Product = new Product()
                    {
                        ProductName = "Product"
                    }
                }
            };

            var customerDetail = new CustomerDetailDto
            {
                CustomerId = "ABC123",
                Email = "Test@email.com",
                FirstName = "Mr",
                LastName = "Blue",
                LastLoggedIn = DateTime.UtcNow.ToString(),
                HouseNumber = "5c",
                Street = "Street",
                Town = "Big Town",
                Postcode = "PO5T C0D3"
            };

            // Act
            var result = apiModelConverter.ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel(orderItems, customerDetail);

            // Assert
            Assert.AreEqual("Mr", result.Customer.FirstName);
            Assert.AreEqual("Blue", result.Customer.LastName);
            Assert.IsTrue(result.Order.OrderItems.Any(x => x.Product == "Product"));
            Assert.AreEqual("5C Street, Big Town, PO5T C0D3", result.Order.DeliveryAddress);
        }
    }
}