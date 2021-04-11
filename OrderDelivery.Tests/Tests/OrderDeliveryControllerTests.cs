using System;
using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderDelivery.Data;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using OrderDelivery.Controllers;
using OrderDelivery.ExternalApi;
using OrderDelivery.Models;
using OrderDelivery.Utils;

namespace OrderDelivery.Tests
{
    [TestClass]
    public class OrderDeliveryControllerTests
    {
        [TestMethod]
        public async Task OrderDetails_InvalidEmailAddress()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Setup CustomerInputModel
                var customerInputModel = new CustomerInputModel
                {
                    CustomerId = "TestName",
                    EmailAddress = "TestEmail"
                };

                // Arrange
                var controller = mock.Create<OrderDeliveryController>();
                
                // Act
                var actionResult = await controller.OrderDetails(customerInputModel);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
                Assert.AreEqual("Invalid Email Address", ((BadRequestErrorMessageResult)actionResult).Message);
            }
        }

        [TestMethod]
        public async Task OrderDetails_UserDoesNotExist()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Setup CustomerInputModel
                var customerInputModel = new CustomerInputModel
                {
                    CustomerId = "TestName",
                    EmailAddress = "test@email.com"
                };

                // Arrange Mock CustomerDetail API
                mock.Mock<ICustomerDetailApi>()
                    .Setup(x => x.GetUserDetailsAsync(It.IsAny<string>()))
                    .ReturnsAsync(null);

                // Create Controller
                var controller = mock.Create<OrderDeliveryController>();
                
                // Act
                var actionResult = await controller.OrderDetails(customerInputModel);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
            }
        }

        [TestMethod]
        public async Task OrderDetails_GetUserDetailApiThrowsUnauthorizedException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Setup CustomerInputModel
                var customerInputModel = new CustomerInputModel
                {
                    CustomerId = "TestName",
                    EmailAddress = "test@email.com"
                };

                // Arrange Mock CustomerDetail API
                mock.Mock<ICustomerDetailApi>()
                    .Setup(x => x.GetUserDetailsAsync(It.IsAny<string>()))
                    .Throws(new HttpResponseException(HttpStatusCode.Unauthorized));

                // Create Controller
                var controller = mock.Create<OrderDeliveryController>();

                // Act
                var actionResult = await controller.OrderDetails(customerInputModel);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(InternalServerErrorResult));
            }
        }

        [TestMethod]
        public async Task OrderDetails_CustomerIdDoesNotMatch()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Setup CustomerInputModel
                var customerInputModel = new CustomerInputModel
                {
                    CustomerId = "TestName",
                    EmailAddress = "test@email.com"
                };

                // Arrange Mock CustomerDetail API
                mock.Mock<ICustomerDetailApi>()
                    .Setup(x => x.GetUserDetailsAsync(It.IsAny<string>()))
                    .ReturnsAsync(new CustomerDetailDto{ CustomerId = "ABC123" });

                // Create Controller
                var controller = mock.Create<OrderDeliveryController>();
                
                // Act
                var actionResult = await controller.OrderDetails(customerInputModel);

                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
                Assert.AreEqual("Invalid Customer Number", ((BadRequestErrorMessageResult)actionResult).Message);
            }
        }

        [TestMethod]
        public async Task OrderDetails_FailedToConvertModel()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Setup CustomerInputModel
                var customerInputModel = new CustomerInputModel
                {
                    CustomerId = "ABC123",
                    EmailAddress = "abc123@email.com"
                };

                // Arrange  Mock CustomerDetail API
                mock.Mock<ICustomerDetailApi>()
                    .Setup(x => x.GetUserDetailsAsync(It.IsAny<string>()))
                    .ReturnsAsync(new CustomerDetailDto { CustomerId = "ABC123", FirstName = "Mr", LastName = "Blue" });

                // Create Controller
                var controller = mock.Create<OrderDeliveryController>();

                // Act
                var actionResult = await controller.OrderDetails(customerInputModel);
                
                // Assert
                Assert.IsInstanceOfType(actionResult, typeof(InternalServerErrorResult));
            }
        }

        [TestMethod]
        public async Task OrderDetails_NoOrdersForCustomer()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Setup CustomerInputModel
                var customerInputModel = new CustomerInputModel
                {
                    CustomerId = "ABC123",
                    EmailAddress = "abc123@email.com"
                };

                // Arrange  Mock CustomerDetail API
                mock.Mock<ICustomerDetailApi>()
                    .Setup(x => x.GetUserDetailsAsync(It.IsAny<string>()))
                    .ReturnsAsync(new CustomerDetailDto { CustomerId = "ABC123", FirstName = "Mr", LastName = "Blue" });

                // Arrange  Mock CustomerDetail API
                mock.Mock<IApiModelConverter>()
                    .Setup(x => x.ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel(It.IsAny<List<OrderItem>>(),
                        It.IsAny<CustomerDetailDto>()))
                    .Returns(new OrderDeliveryModel()
                    {
                        Customer = new CustomerModel()
                        {
                            FirstName = "Mr",
                            LastName = "Blue"
                        }
                    });

                // Create Controller
                var controller = mock.Create<OrderDeliveryController>();

                // Act
                var actionResult = await controller.OrderDetails(customerInputModel);
                var contentResult = actionResult as OkNegotiatedContentResult<OrderDeliveryModel>;

                // Assert
                Assert.IsNotNull(contentResult);
                Assert.IsNotNull(contentResult.Content);
                Assert.AreEqual("Mr", contentResult.Content.Customer.FirstName);
                Assert.AreEqual("Blue", contentResult.Content.Customer.LastName);
                Assert.IsNull(contentResult.Content.Order);
            }
        }

        [TestMethod]
        public async Task OrderDetails_ValidData()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Setup CustomerInputModel
                var customerInputModel = new CustomerInputModel
                {
                    CustomerId = "ABC123",
                    EmailAddress = "abc123@email.com"
                };

                // Arrange  Mock CustomerDetail API
                mock.Mock<ICustomerDetailApi>()
                    .Setup(x => x.GetUserDetailsAsync(It.IsAny<string>()))
                    .ReturnsAsync(new CustomerDetailDto { CustomerId = "ABC123", FirstName = "Mr", LastName = "Blue" });

                // Arrange  Mock CustomerDetail API
                mock.Mock<IApiModelConverter>()
                    .Setup(x => x.ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel(It.IsAny<List<OrderItem>>(),
                        It.IsAny<CustomerDetailDto>()))
                    .Returns(new OrderDeliveryModel()
                    {
                        Customer = new CustomerModel()
                        {
                            FirstName = "Mr",
                            LastName = "Blue"
                        },
                        Order = new OrderModel()
                        {
                            OrderNumber = 500
                        }
                    });

                // Create Controller
                var controller = mock.Create<OrderDeliveryController>();

                // Act
                var actionResult = await controller.OrderDetails(customerInputModel);
                var contentResult = actionResult as OkNegotiatedContentResult<OrderDeliveryModel>;

                // Assert
                Assert.IsNotNull(contentResult);
                Assert.IsNotNull(contentResult.Content);
                Assert.AreEqual("Mr", contentResult.Content.Customer.FirstName);
                Assert.AreEqual("Blue", contentResult.Content.Customer.LastName);
                Assert.AreEqual(500, contentResult.Content.Order.OrderNumber);
            }
        }
    }
}
