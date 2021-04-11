using System.Data.Entity;

namespace OrderDelivery.Data
{
    public class OrderDeliveryContext : DbContext
    {
        public OrderDeliveryContext() : base("MMTOrderDeliveryConnectionString")
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}