using System.ComponentModel.DataAnnotations;

namespace OrderDelivery.Data
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [StringLength(50)]
        public string ProductName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##}")]
        public decimal PackHeight { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##}")]
        public decimal PackWidth { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.###}")]
        public decimal? PackWeight { get; set; }

        [StringLength(20)]
        public string Colour { get; set; }

        [StringLength(20)]
        public string Size { get; set; }
    }
}