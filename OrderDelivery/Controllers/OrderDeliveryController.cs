using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using OrderDelivery.Data;
using OrderDelivery.ExternalApi;
using OrderDelivery.Models;
using OrderDelivery.Utils;

namespace OrderDelivery.Controllers
{
    public class OrderDeliveryController : ApiController
    {
        private readonly ICustomerDetailApi _customerDetailApi;
        private readonly IOrderDeliveryRepository _orderDeliveryRepository;
        private readonly IApiModelConverter _apiModelConverter;

        public OrderDeliveryController(ICustomerDetailApi customerDetailApi, IOrderDeliveryRepository orderDeliveryRepository, IApiModelConverter apiModelConverter)
        {
            _customerDetailApi = customerDetailApi ?? throw new ArgumentNullException(nameof(ICustomerDetailApi));
            _orderDeliveryRepository = orderDeliveryRepository ?? throw new ArgumentNullException(nameof(IOrderDeliveryRepository));
            _apiModelConverter = apiModelConverter ?? throw new ArgumentNullException(nameof(IApiModelConverter));
        }

        [HttpPost]
        [Route("api/OrderDetails")]
        public async Task<IHttpActionResult> OrderDetails(CustomerInputModel customerInputModel)
        {
            Trace.TraceInformation($"{nameof(OrderDetails)} called");
            try
            {
                if (ModelState.IsValid)
                {
                    // Verify Email Address
                    if (!IsValidEmail(customerInputModel.EmailAddress))
                        return BadRequest("Invalid Email Address");

                    // Retrieve User details
                    var customerDetail = await _customerDetailApi.GetUserDetailsAsync(customerInputModel.EmailAddress);
                    if (customerDetail == null)
                        return NotFound();

                    // Verify Customer number
                    if (customerDetail.CustomerId != customerInputModel.CustomerId)
                        return BadRequest("Invalid Customer Number");

                    // Retrieve order details from DB
                    var latestOrderItemsForCustomer = await _orderDeliveryRepository.GetLatestOrderItemsForCustomerAsync(customerInputModel.CustomerId);
                    
                    var result =
                        _apiModelConverter.ConvertOrderItemsAndCustomerInfoToOrderDeliveryModel(
                            latestOrderItemsForCustomer, customerDetail);
                    
                    if (result == null)
                        throw new InvalidOperationException("Failed to convert Model");

                    return Ok(result);
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                return InternalServerError();
            }

            // Invalid input model
            return BadRequest();
        }

        #region Helper functions

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}