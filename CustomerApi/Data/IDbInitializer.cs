using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomersApi.Data
{
    interface IDbInitializer
    {
        void Initialize(CustomerApiContext dbContext);
    }
}
