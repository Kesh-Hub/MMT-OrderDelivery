using Newtonsoft.Json;

namespace OrderDelivery.Data
{
    public class CustomerDetailDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("customerId")]
        public string CustomerId { get; set; }

        [JsonProperty("website")]
        public bool Website { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("lastLoggedIn")]
        public string LastLoggedIn { get; set; }

        [JsonProperty("houseNumber")]
        public string HouseNumber { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("town")]
        public string Town { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("preferredLanguage")]
        public string PreferredLanguage { get; set; }
    }
}