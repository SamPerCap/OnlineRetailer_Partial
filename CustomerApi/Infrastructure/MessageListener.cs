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
                bus.Respond<SharedCustomers, SharedCustomers>(message => HandleCustomerExists(message.Id));

                lock (this)
                    Monitor.Wait(this);
            }
        }

        private SharedCustomers HandleCustomerExists(int message)
        {
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var custoerRepo = services.GetService<IRepository<SharedCustomers>>();
                var customer = custoerRepo.Get(message);
                return customer;
            }
        }
    }
}
