using OrderApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Infastructure
{
    public class ProductServiceGateway : IServiceGateway<HiddenProduct>
    {
        Uri productServiceBaseUrl;

        public ProductServiceGateway(Uri baseUrl)
        {
            productServiceBaseUrl = baseUrl;
        }
        public HiddenProduct Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
