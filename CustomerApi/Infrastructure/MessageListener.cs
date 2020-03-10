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
                //It needs to be adapted to customers
                //bus.Subscribe<OrderStatusChangedMessage>("customerApiCompleted",
                 //   HandleOrderCompleted, x => x.WithTopic("completed"));

                // Block the thread so that it will not exit and stop subscribing.
                lock (this)
                {
                    Monitor.Wait(this);
                }
            }

        }

        
    }
}
