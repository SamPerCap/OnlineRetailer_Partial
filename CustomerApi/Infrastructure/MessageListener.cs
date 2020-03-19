using CustomersApi.Data;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerApi.Infrastructure
{
    public class MessageListener
    {
        IServiceProvider provider;
        string connectionString;

        public MessageListener(IServiceProvider provider, string connectionString)
        {
            this.provider = provider;
            this.connectionString = connectionString;
        }

        public void Start()
        {
            using (var bus = RabbitHutch.CreateBus(connectionString))
            {
                bus.Respond<SharedCustomerRequest, SharedCustomerResponse>(request => CustomerExists(request));


                // Block the thread so that it will not exit and stop subscribing.
                lock (this)
                {
                    Monitor.Wait(this);
                }
            }

        }

        private SharedCustomerResponse CustomerExists(SharedCustomerRequest request)
        {
            // A service scope is created to get an instance of the product repository.
            // When the service scope is disposed, the product repository instance will
            // also be disposed.
            using (var scope = provider.CreateScope())
            {
                var response = new SharedCustomerResponse();
                var services = scope.ServiceProvider;
                var customerRepos = services.GetService<IRepository<SharedCustomers>>();

                var customer = customerRepos.Get(request.CustomerId);
                if (customer != null)
                {
                    response.Exists = true;
                }
                else
                {
                    response.Exists = false;
                }
                return response;
            }
        }
    }
}
