using Microsoft.EntityFrameworkCore;
using SharedModels;

namespace CustomersApi.Data
{
    public class CustomerApiContext : DbContext
    {
        public CustomerApiContext(DbContextOptions<CustomerApiContext> options)
            : base(options)
        {

        }

        public DbSet<SharedCustomers> Customers { get; set; }
    }
}
