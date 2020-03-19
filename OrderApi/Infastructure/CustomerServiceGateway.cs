using RestSharp;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Infastructure
{

    public class CustomerServiceGateway : IServiceGateway<SharedCustomers>
    {

        Uri customerServiceBaseUrl;

        public CustomerServiceGateway(Uri baseUrl)
        {
            customerServiceBaseUrl = baseUrl;
        }
        public SharedCustomers Get(int id)
        {
            RestClient c = new RestClient();
            c.BaseUrl = customerServiceBaseUrl;

            var request = new RestRequest(id.ToString(), Method.GET);
            var response = c.Execute<SharedCustomers>(request);
            var orderedCustomer = response.Data;
            return orderedCustomer;
        }
    }
}
