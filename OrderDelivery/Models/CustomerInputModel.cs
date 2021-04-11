using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OrderDelivery.Models
{
    public class CustomerInputModel
    {
        [Required]
        [JsonProperty("user")]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(10)]
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
    }
}