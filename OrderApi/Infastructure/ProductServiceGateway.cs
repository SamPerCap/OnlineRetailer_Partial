using OrderApi.Models;
using RestSharp;
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
            RestClient c = new RestClient();
            c.BaseUrl = productServiceBaseUrl;

            var request = new RestRequest(id.ToString(), Method.GET);
            var response = c.Execute<SharedProducts>(request);
            var orderedProduct = response.Data;
            return orderedProduct;
        }
    }
}
