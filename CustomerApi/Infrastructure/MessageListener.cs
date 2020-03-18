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
                var subscriptionResult = bus.Subscribe<SharedCustomers>("customerExists",
                    HandleCustomerExists, x => x.WithTopic("Exists"));
            }
        }

        private void HandleCustomerExists(SharedCustomers message)
        {
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var custoerRepo = services.GetService<IRepository<SharedCustomers>>();
                var customer = custoerRepo.Get(message.Id);
            }
        }
    }
}
