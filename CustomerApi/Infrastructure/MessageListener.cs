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
                bus.Respond<SharedCustomerRequest, SharedCustomerResponse>(message => HandleCustomerExists(message));

                lock (this)
                    Monitor.Wait(this, 8000);
            }
        }

        private SharedCustomerResponse HandleCustomerExists(SharedCustomerRequest message)
        {
            // A service scope is created to get an instance of the product repository.
            // When the service scope is disposed, the product repository instance will
            // also be disposed.

            using (var scope = provider.CreateScope())
            {
                SharedCustomerResponse response = new SharedCustomerResponse();

                var services = scope.ServiceProvider;
                var customerRepo = services.GetService<IRepository<SharedCustomers>>();

                var cust = customerRepo.Get(message.CustomerId);

                if (cust.CreditStanding > 0)
                {
                    response.Exists = true;
                }
                else
                {
                    response = null;
                    response.Exists = false;
                }
                return response;
            }
        }

        //private SharedCustomers HandleCustomerExists(int message)
        //{
        //    using (var scope = provider.CreateScope())
        //    {
        //        var services = scope.ServiceProvider;
        //        var custoerRepo = services.GetService<IRepository<SharedCustomers>>();
        //        var customer = custoerRepo.Get(message);
        //        return customer;
        //    }
        //}
    }
}
