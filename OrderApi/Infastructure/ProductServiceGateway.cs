using OrderApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Infastructure
{
    public class ProductServiceGateway : IServiceGateway<Product>
    {
        Uri productServiceBaseUrl;

        public ProductServiceGateway(Uri baseUrl)
        {
            productServiceBaseUrl = baseUrl;
        }
        public Product Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
