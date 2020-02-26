using Microsoft.EntityFrameworkCore;
using OrderApi.Models;

namespace OrderApi.Data
{
    public class OrderApiContext : DbContext
    {
        public OrderApiContext(DbContextOptions<OrderApiContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                 .HasOne(p => p.Product)
                 .WithOne(u => u.Order)
                 .IsRequired();

            modelBuilder.Entity<Customer>()
                .HasMany(p => p.Order)
                .WithOne(u => u.Customer)
                .IsRequired();
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
