using OrderApi.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Infastructure
{
    public class ProductServiceGateway : IServiceGateway<SharedProducts>
    {
        Uri productServiceBaseUrl;

        public ProductServiceGateway(Uri baseUrl)
        {
            productServiceBaseUrl = baseUrl;
        }
        public SharedProducts Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
