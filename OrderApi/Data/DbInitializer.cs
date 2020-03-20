using System.Collections.Generic;
using System.Linq;
using OrderApi.Models;
using System;
using SharedModels;

namespace OrderApi.Data
{
    public class DbInitializer : IDbInitializer
    {
        // This method will create and seed the database.
        public void Initialize(OrderApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Products
            if (context.Orders.Any())
            {
                return;   // DB has been seeded
            }

            List<SharedOrders> orders = new List<SharedOrders>
            {
                new SharedOrders { customerId = 1, Date = DateTime.Today,
                    OrderLines = new List<SharedOrderLine>{
                        new SharedOrderLine { ProductId = 1, Quantity = 2, OrderId = 1 }
                    }
                    }
            };

            context.Orders.AddRange(orders);
            context.SaveChanges();
        }
    }
}
