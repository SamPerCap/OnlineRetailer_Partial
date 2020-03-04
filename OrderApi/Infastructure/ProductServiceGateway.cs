using OrderApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Infastructure
{
    public class ProductServiceGateway : IServiceGateway<HiddenProduct>
    {
        Uri productServiceBaseUrl;

        public ProductServiceGateway(Uri baseUrl)
        {
            productServiceBaseUrl = baseUrl;
        }

        HiddenProduct IServiceGateway<HiddenProduct>.Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
