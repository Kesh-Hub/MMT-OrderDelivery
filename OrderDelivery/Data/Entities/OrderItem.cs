using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderDelivery.Data
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }
        
        public int Quantity { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##}")]
        public decimal? Price { get; set; }

        public bool? Returnable { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}