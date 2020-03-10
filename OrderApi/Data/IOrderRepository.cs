using OrderApi.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Data
{
    public interface IOrderRepository : IRepository<SharedOrders>
    {
        IEnumerable<SharedOrders> GetByCustomer(int customerId);
    }
}
