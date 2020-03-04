using CustomersApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomersApi.Data
{
    public class DbInitializer : IDbInitializer
    {
        public void Initialize(CustomerApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Products
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            List<HiddenCustomer> customers = new List<HiddenCustomer>
            {
                new HiddenCustomer { Name = "Customer0", Email = "customer0@hotmail.com", BillingAddress = "Wuhan's first street", Phone = 666, CreditStanding = 15, ShippingAddress = "Corona Street" },
                new HiddenCustomer { Name = "Customer1", Email = "customer1@hotmail.com", BillingAddress = "Somalia's latest street", Phone = 0101, CreditStanding = 2395123, ShippingAddress = "Mogadiscio" },
                new HiddenCustomer { Name = "Customer2", Email = "customer2@hotmail.com", BillingAddress = "EASV", Phone = 1453, CreditStanding = 0, ShippingAddress = "SDU" }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}
