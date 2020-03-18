using EasyNetQ;
using SharedModels;
using System;
using System.Threading;

namespace OrderApi.Infastructure
{
    public class MessageListener
    {
        IServiceProvider provider;
        string connectionString;
        bool orderExists;

        // The service provider is passed as a parameter, because the class needs
        // access to the product repository. With the service provider, we can create
        // a service scope that can provide an instance of the product repository.
        public MessageListener(IServiceProvider provider, string connectionString, bool _orderExists)
        {
            orderExists = _orderExists;
            this.provider = provider;
            this.connectionString = connectionString;
        }

        public void Start()
        {
            using (var bus = RabbitHutch.CreateBus(connectionString))
            {
                bus.Receive<SharedProducts>("productIsAvailable", message => HandleOrderRequest(message));
                // Block the thread so that it will not exit and stop subscribing.
                lock (this)
                {
                    Monitor.Wait(this);
                }
            }

        }

        public bool DoesOrderExist()
        {
            return orderExists;
        }

        private void HandleOrderRequest(SharedProducts message)
        {
            if (message != null)
            {
                orderExists = true;
            }
            else
            {
                orderExists = false;
            }
        }
    }
}
