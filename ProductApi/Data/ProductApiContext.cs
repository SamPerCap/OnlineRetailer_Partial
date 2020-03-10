using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using SharedModels;

namespace ProductApi.Data
{
    public class ProductApiContext : DbContext
    {
        public ProductApiContext(DbContextOptions<ProductApiContext> options)
            : base(options)
        {
        }

        public DbSet<SharedProducts> Products { get; set; }
    }
}
